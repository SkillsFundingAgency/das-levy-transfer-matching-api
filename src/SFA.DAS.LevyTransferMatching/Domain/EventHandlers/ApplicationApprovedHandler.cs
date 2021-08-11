using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationApprovedHandler : IDomainEventHandler<ApplicationApproved>
    {
        public Task Handle(ApplicationApproved @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
