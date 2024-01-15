using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NServiceBus;
using SFA.DAS.LevyTransferMatching.Messages.Commands;

namespace SFA.DAS.LevyTransferMatching.Api.HealthChecks
{
    public class NServiceBusHealthCheck : IHealthCheck
    {
        // This should match the QueueNames.RunHealthCheck value in the  SFA.DAS.LevyTransferMatching.Functions project. We may want to add it to the shared SFA.DAS.LevyTransferMatching.Messages nuget
        private const string HealthCheckQueueName = "SFA.DAS.LevyTransferMatching.HealthCheck";

        public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(500);
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

        private readonly IMessageSession _messageSession;
        private readonly IDistributedCache _distributedCache;

        public NServiceBusHealthCheck(IMessageSession messageSession, IDistributedCache distributedCache)
        {
            _messageSession = messageSession;
            _distributedCache = distributedCache;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var messageId = Guid.NewGuid();
            var data = new Dictionary<string, object> { ["MessageId"] = messageId };
            var stopwatch = Stopwatch.StartNew();

            await _messageSession.Send(HealthCheckQueueName, (RunHealthCheckCommand c) => c.MessageId = messageId.ToString());

            while (!cancellationToken.IsCancellationRequested)
            {
                if (await _distributedCache.GetStringAsync(messageId.ToString(), cancellationToken) != null)
                {
                    return HealthCheckResult.Healthy(null, data);
                }

                if (stopwatch.Elapsed > Timeout)
                {
                    return HealthCheckResult.Degraded($"Potential issue with receiving endpoint (failed to handle {nameof(RunHealthCheckCommand)} in sufficient time)", null, data);
                }

                await Task.Delay(Interval, cancellationToken);
            }

            throw new OperationCanceledException(cancellationToken);
        }
    }
}
