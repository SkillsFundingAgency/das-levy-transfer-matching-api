using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeClosedEventHandler(IEventPublisher eventPublisher) : IDomainEventHandler<PledgeClosed>
{
    public async Task Handle(PledgeClosed @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(new PledgeClosedEvent(@event.PledgeId, @event.InsufficientFunds));
    }
}