using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;

namespace SFA.DAS.LevyTransferMatching.Functions.TestHarness
{
    internal class Program
    {
        private const string EndpointName = "SFA.DAS.LevyTransferMatching.Function.TestHarness";
        private const string ConfigName = "SFA.DAS.LevyTransferMatching";

        public static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .AddAzureTableStorage(ConfigName);

            var configuration = builder.Build();

            var endpointConfiguration = new EndpointConfiguration(EndpointName)
                .UseErrorQueue($"{EndpointName}-errors")
                .UseInstallers()
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer();

            if (configuration[$"{ConfigName}:NServiceBusConnectionString"] == "UseDevelopmentStorage=true")
            {
                endpointConfiguration.UseTransport<LearningTransport>().StorageDirectory(
                    Path.Combine(
                        Directory.GetCurrentDirectory()
                            .Substring(0, Directory.GetCurrentDirectory().IndexOf("src")),
                        @"src\SFA.DAS.LevyTransferMatching.Functions\.learningtransport"));
            }

            var endpoint = await Endpoint.Start(endpointConfiguration);
            var testHarness = new TestHarness(endpoint);

            await testHarness.Run();
            await endpoint.Stop();
        }
    }
}
