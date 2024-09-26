using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationWithdrawnAfterAcceptanceHandler(IEventPublisher eventPublisher) : IDomainEventHandler<ApplicationWithdrawnAfterAcceptance>
{
    public async Task Handle(ApplicationWithdrawnAfterAcceptance @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(new ApplicationWithdrawnAfterAcceptanceEvent(@event.ApplicationId, @event.PledgeId, @event.Amount));
    }
}