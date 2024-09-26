using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationCreatedHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    : IDomainEventHandler<ApplicationCreated>
{
    public async Task Handle(ApplicationCreated @event, CancellationToken cancellationToken = default)
    {
        var pledge = await pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await eventPublisher.Publish(new ApplicationCreatedEvent(@event.ApplicationId, @event.PledgeId,
            @event.CreatedOn,
            senderId,
            @event.ReceiverAccountId));
    }
}