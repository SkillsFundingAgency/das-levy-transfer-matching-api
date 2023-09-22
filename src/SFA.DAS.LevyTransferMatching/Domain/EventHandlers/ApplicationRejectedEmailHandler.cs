using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationRejectedEmailHandler : IDomainEventHandler<ApplicationRejectedEmail>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationRejectedEmailHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(ApplicationRejectedEmail @event, CancellationToken cancellationToken = default)
        {
            await _eventPublisher.Publish(new ApplicationRejectedEmailEvent(@event.ApplicationId, @event.PledgeId, @event.ReceiverAccountId));
        }
    }
}
