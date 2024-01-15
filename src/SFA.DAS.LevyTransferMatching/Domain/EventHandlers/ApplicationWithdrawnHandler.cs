using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class ApplicationWithdrawnHandler : IDomainEventHandler<ApplicationWithdrawn>
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IPledgeRepository _pledgeRepository;

    public ApplicationWithdrawnHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    {
        _eventPublisher = eventPublisher;
        _pledgeRepository = pledgeRepository;
    }

    public async Task Handle(ApplicationWithdrawn @event, CancellationToken cancellationToken = default)
    {
        var pledge = await _pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await _eventPublisher.Publish(new ApplicationWithdrawnEvent(@event.ApplicationId, @event.PledgeId,
            @event.WithdrawnOn,
            senderId));
    }
}