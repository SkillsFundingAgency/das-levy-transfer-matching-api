using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Functions.Configuration;
using SFA.DAS.NServiceBus.AzureFunction.Configuration;
using SFA.DAS.NServiceBus.AzureFunction.Hosting;

namespace SFA.DAS.LevyTransferMatching.Functions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNLog(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging((options) =>
            {
                options.AddFilter(typeof(Startup).Namespace, LogLevel.Information);
                options.SetMinimumLevel(LogLevel.Trace);
                options.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
                options.AddConsole();

                NLogConfiguration.ConfigureNLog();
            });

            return serviceCollection;
        }

        public static IServiceCollection AddNServiceBus(
            this IServiceCollection serviceCollection,
            ILogger logger,
            Action<NServiceBusOptions> OnConfigureOptions = null)
        {
            serviceCollection.AddSingleton<IExtensionConfigProvider, NServiceBusExtensionConfigProvider>((c) =>
            {
                var options = new NServiceBusOptions
                {
                    OnMessageReceived = (context) =>
                    {
                        context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out string messageType);
                        context.Headers.TryGetValue("NServiceBus.MessageId", out string messageId);
                        context.Headers.TryGetValue("NServiceBus.CorrelationId", out string correlationId);
                        context.Headers.TryGetValue("NServiceBus.OriginatingEndpoint", out string originatingEndpoint);
                        logger.LogInformation($"Received NServiceBusTriggerData Message of type '{(messageType != null ? messageType.Split(',')[0] : string.Empty)}' with messageId '{messageId}' and correlationId '{correlationId}' from endpoint '{originatingEndpoint}'");

                    },
                    OnMessageErrored = (ex, context) =>
                    {
                        context.Headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out string messageType);
                        context.Headers.TryGetValue("NServiceBus.MessageId", out string messageId);
                        context.Headers.TryGetValue("NServiceBus.CorrelationId", out string correlationId);
                        context.Headers.TryGetValue("NServiceBus.OriginatingEndpoint", out string originatingEndpoint);
                        logger.LogError(ex, $"Error handling NServiceBusTriggerData Message of type '{(messageType != null ? messageType.Split(',')[0] : string.Empty)}' with messageId '{messageId}' and correlationId '{correlationId}' from endpoint '{originatingEndpoint}'");
                    }
                };

                if (OnConfigureOptions != null)
                {
                    OnConfigureOptions.Invoke(options);
                }

                return new NServiceBusExtensionConfigProvider(options);
            });

            return serviceCollection;
        }
    }
}
