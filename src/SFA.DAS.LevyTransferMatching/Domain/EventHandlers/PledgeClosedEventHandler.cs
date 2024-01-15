using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;
using System.Threading.Tasks;
using System.Threading;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeClosedEventHandler : IDomainEventHandler<PledgeClosed>
{
    private readonly IEventPublisher _eventPublisher;

    public PledgeClosedEventHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(PledgeClosed @event, CancellationToken cancellationToken = default)
    {
        await _eventPublisher.Publish(new PledgeClosedEvent(@event.PledgeId, @event.InsufficientFunds));
    }
}