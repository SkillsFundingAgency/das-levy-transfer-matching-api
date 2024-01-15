using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeDebitFailedHandler : IDomainEventHandler<PledgeDebitFailed>
{
    private readonly IEventPublisher _eventPublisher;

    public PledgeDebitFailedHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(PledgeDebitFailed @event, CancellationToken cancellationToken = default)
    {
        await _eventPublisher.Publish(new PledgeDebitFailedEvent(@event.PledgeId, @event.ApplicationId, @event.Amount));
    }
}