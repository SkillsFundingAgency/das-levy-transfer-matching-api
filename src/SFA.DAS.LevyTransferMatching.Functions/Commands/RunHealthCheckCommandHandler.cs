using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Messages.Commands;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;

namespace SFA.DAS.LevyTransferMatching.Functions.Commands
{
    public class RunHealthCheckCommandHandler
    {
        private readonly IDistributedCache _distributedCache;

        public RunHealthCheckCommandHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [FunctionName("RunHealthCheckCommand")]
        public async Task Run([NServiceBusTrigger(Endpoint = QueueNames.RunHealthCheck)] RunHealthCheckCommand runHealthCheck, ILogger log)
        {
            log.LogInformation($"Handling command: {runHealthCheck}");

            await _distributedCache.SetStringAsync(runHealthCheck.MessageId, "OK");
        }
    }
}
