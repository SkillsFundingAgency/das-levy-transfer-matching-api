using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions;

public static class NServiceBusStartup
{
    private const string EndpointName = "SFA.DAS.LevyTransferMatching.MessageHandlers";

    public static void StartNServiceBus(this UpdateableServiceProvider serviceProvider, LevyTransferMatchingApi configuration)
    {
        var connectionFactory = serviceProvider.GetService<IConnectionFactory>();

        var endpointConfiguration = new EndpointConfiguration(EndpointName)
            .UseErrorQueue($"{EndpointName}-errors")
            .UseMessageConventions()
            .UseNewtonsoftJsonSerializer()
            .UseOutbox(true)
            .UseServicesBuilder(serviceProvider)
            .UseSqlServerPersistence(() => connectionFactory.CreateConnection(configuration.DatabaseConnectionString))
            .UseUnitOfWork();

        if (configuration.NServiceBusConnectionString.Equals("UseDevelopmentStorage=true", StringComparison.CurrentCultureIgnoreCase))
        {
            endpointConfiguration.UseLearningTransport(s => s.AddRouting());
        }
        else
        {
            endpointConfiguration.UseAzureServiceBusTransport(configuration.NServiceBusConnectionString, r => { r.AddRouting(); });
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