using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Functions.StartupExtensions
{
    public static class NServiceBusStartupExtensions
    {
        public static IServiceCollection AddNServiceBus(this IServiceCollection services, LevyTransferMatchingFunctions config, ILogger logger)
        {
            if (config.NServiceBusConnectionString.Equals("UseDevelopmentStorage=true", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddNServiceBus(logger, (options) =>
                {
                    options.EndpointConfiguration = (endpoint) =>
                    {
                        endpoint.UseTransport<LearningTransport>().StorageDirectory(
                            Path.Combine(
                                Directory.GetCurrentDirectory()
                                    .Substring(0, Directory.GetCurrentDirectory().IndexOf("src")),
                                @"src\.learningtransport"));
                        
                        return endpoint;
                    };
                });
            }
            else
            {
                services.AddNServiceBus(logger);
            }

            return services;
        }
    }
}
