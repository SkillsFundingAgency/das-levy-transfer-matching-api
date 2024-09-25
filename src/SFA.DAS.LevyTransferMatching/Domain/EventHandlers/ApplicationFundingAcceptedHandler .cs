using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationFundingAcceptedHandler(IEventPublisher eventPublisher) : IDomainEventHandler<ApplicationFundingAccepted>
{
    public async Task Handle(ApplicationFundingAccepted @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(
            new ApplicationFundingAcceptedEvent(
                @event.ApplicationId,
                @event.PledgeId,                   
                @event.RejectApplications));
    }
}