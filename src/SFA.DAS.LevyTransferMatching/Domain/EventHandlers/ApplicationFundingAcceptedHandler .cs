using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationFundingAcceptedHandler : IDomainEventHandler<ApplicationFundingAccepted>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationFundingAcceptedHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(ApplicationFundingAccepted @event, CancellationToken cancellationToken = default)
        {
            await _eventPublisher.Publish(
                new ApplicationFundingAcceptedEvent(
                    @event.ApplicationId,
                    @event.PledgeId,                   
                    @event.RejectApplications));
        }
    }
}