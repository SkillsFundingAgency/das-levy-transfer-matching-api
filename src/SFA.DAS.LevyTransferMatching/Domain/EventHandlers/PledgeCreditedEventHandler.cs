using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;
using System.Threading.Tasks;
using System.Threading;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers;

public class PledgeCreditedEventHandler : IDomainEventHandler<PledgeCredited>
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IPledgeRepository _pledgeRepository;

    public PledgeCreditedEventHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository)
    {
        _eventPublisher = eventPublisher;
        _pledgeRepository = pledgeRepository;
    }

    public async Task Handle(PledgeCredited @event, CancellationToken cancellationToken = default)
    {
        var pledge = await _pledgeRepository.Get(@event.PledgeId);

        var senderId = pledge.EmployerAccountId;

        await _eventPublisher.Publish(new PledgeCreditedEvent(@event.PledgeId, senderId));
    }
}