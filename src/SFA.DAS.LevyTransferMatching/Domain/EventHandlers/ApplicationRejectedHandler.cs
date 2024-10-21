using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationRejectedHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    : IDomainEventHandler<ApplicationRejected>
{
    public async Task Handle(ApplicationRejected @event, CancellationToken cancellationToken = default)
    {
        var pledge = await pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await eventPublisher.Publish(new ApplicationRejectedEvent(@event.ApplicationId, @event.PledgeId,
            @event.RejectedOn,
            @event.Amount,
            senderId,
            @event.ReceiverAccountId));
    }
}