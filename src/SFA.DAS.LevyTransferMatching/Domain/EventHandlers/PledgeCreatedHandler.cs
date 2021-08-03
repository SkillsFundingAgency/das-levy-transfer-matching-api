using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class PledgeCreatedHandler : IDomainEventHandler<PledgeCreated>
    {
        public Task Handle(PledgeCreated @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            var pledge = @event.Pledge;
            return Task.CompletedTask;
        }
    }
}
