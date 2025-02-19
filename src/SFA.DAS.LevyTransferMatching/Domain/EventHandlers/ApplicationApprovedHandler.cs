﻿using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationApprovedHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    : IDomainEventHandler<ApplicationApproved>
{
    public async Task Handle(ApplicationApproved @event, CancellationToken cancellationToken = default)
    {
        var pledge = await pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await eventPublisher.Publish(new ApplicationApprovedEvent(@event.ApplicationId, @event.PledgeId,
            @event.ApprovedOn,
            @event.Amount,
            senderId,
            @event.ReceiverAccountId));
    }
}