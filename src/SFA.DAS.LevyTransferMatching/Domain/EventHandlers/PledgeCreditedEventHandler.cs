using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeCreditedEventHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    : IDomainEventHandler<PledgeCredited>
{
    public async Task Handle(PledgeCredited @event, CancellationToken cancellationToken = default)
    {
        var pledge = await pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await eventPublisher.Publish(new PledgeCreditedEvent(@event.PledgeId, senderId));
    }
}