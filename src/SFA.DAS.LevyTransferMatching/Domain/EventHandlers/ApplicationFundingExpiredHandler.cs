using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationFundingExpiredHandler(IEventPublisher eventPublisher) : IDomainEventHandler<ApplicationFundingExpired>
{
    public async Task Handle(ApplicationFundingExpired @event, CancellationToken cancellationToken = default)
    {
        await eventPublisher.Publish(
            new ApplicationFundingExpiredEvent(
                @event.ApplicationId));
    }
}