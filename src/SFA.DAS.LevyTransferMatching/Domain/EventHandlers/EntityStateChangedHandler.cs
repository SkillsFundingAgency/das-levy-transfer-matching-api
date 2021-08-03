using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class EntityStateChangedHandler : IDomainEventHandler<EntityStateChanged>
    {
        public Task Handle(EntityStateChanged @event, CancellationToken cancellationToken = default)
        {


            return Task.CompletedTask;

        }
    }
}
