using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationFundingDeclinedHandler(IEventPublisher eventPublisher) : IDomainEventHandler<ApplicationFundingDeclined>
{
    public async Task Handle(ApplicationFundingDeclined @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(
            new ApplicationFundingDeclinedEvent(
                @event.ApplicationId,
                @event.PledgeId,
                @event.DeclinedOn,
                @event.Amount));
    }
}