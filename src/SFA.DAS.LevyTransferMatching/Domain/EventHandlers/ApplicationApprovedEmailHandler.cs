using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationApprovedEmailHandler : IDomainEventHandler<ApplicationApprovedEmail>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationApprovedEmailHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(ApplicationApprovedEmail @event, CancellationToken cancellationToken = default)
        {
            await _eventPublisher.Publish(new ApplicationApprovedEmailEvent(@event.ApplicationId, @event.PledgeId, @event.ReceiverAccountId));
        }
    }
}
