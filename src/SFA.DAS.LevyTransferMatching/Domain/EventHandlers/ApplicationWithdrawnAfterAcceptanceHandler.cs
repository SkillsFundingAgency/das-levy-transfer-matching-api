using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationWithdrawnAfterAcceptanceHandler : IDomainEventHandler<ApplicationWithdrawnAfterAcceptance>
{
    private readonly IEventPublisher _eventPublisher;

    public ApplicationWithdrawnAfterAcceptanceHandler(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(ApplicationWithdrawnAfterAcceptance @event, CancellationToken cancellationToken = default)
    {
        await _eventPublisher.Publish(new ApplicationWithdrawnAfterAcceptanceEvent(@event.ApplicationId, @event.PledgeId, @event.Amount));
    }
}