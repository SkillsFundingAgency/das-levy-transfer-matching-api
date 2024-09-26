using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeDebitFailedHandler(IEventPublisher eventPublisher) : IDomainEventHandler<PledgeDebitFailed>
{
    public async Task Handle(PledgeDebitFailed @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(new PledgeDebitFailedEvent(@event.PledgeId, @event.ApplicationId, @event.Amount));
    }
}