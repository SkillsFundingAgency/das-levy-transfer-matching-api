using NServiceBus;
using SFA.DAS.LevyTransferMatching.Messages.Commands;

namespace SFA.DAS.LevyTransferMatching.Extensions
{
    public static class RoutingSettingsExtensions
    {
        private const string LevyTransferMatchingMessageHandler = "SFA.DAS.LevyTransferMatching.MessageHandlers";


        public static void AddRouting(this RoutingSettings routingSettings)
        {
            routingSettings.RouteToEndpoint(typeof(RunHealthCheckCommand), LevyTransferMatchingMessageHandler);
        }
    }
}
