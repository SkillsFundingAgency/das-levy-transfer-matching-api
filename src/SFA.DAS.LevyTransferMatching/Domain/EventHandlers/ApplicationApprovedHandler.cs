using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationApprovedHandler : IDomainEventHandler<ApplicationApproved>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationApprovedHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(ApplicationApproved @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _eventPublisher.Publish(new ApplicationApprovedEvent(@event.ApplicationId,
                @event.ApprovedOn,
                @event.Amount));
        }
    }
}
