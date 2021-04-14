using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class NServiceBusStartup
    {
        private const string EndpointName = "SFA.DAS.LevyTransferMatching.Api";

        public static void StartNServiceBus(this UpdateableServiceProvider serviceProvider, LevyTransferMatchingApi configuration, IWebHostEnvironment environment)
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName)
                .UseErrorQueue($"{EndpointName}-errors")
                .UseInstallers()
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseOutbox(true)
                .UseServicesBuilder(serviceProvider)
                .UseSqlServerPersistence(() => new SqlConnection(configuration.DatabaseConnectionString))
                .UseUnitOfWork();

            if (environment.IsDevelopment())
            {
                endpointConfiguration.UseLearningTransport();
            }
            else
            {
                endpointConfiguration.UseAzureServiceBusTransport(configuration.NServiceBusConnectionString, r => { });
            }

            if (!string.IsNullOrEmpty(configuration.NServiceBusLicense))
            {
                endpointConfiguration.License(configuration.NServiceBusLicense);
            }

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            serviceProvider.AddSingleton(p => endpoint)
                .AddSingleton<IMessageSession>(p => p.GetService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();
        }
    }
}
