using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationCreatedEmailHandler : IDomainEventHandler<ApplicationCreatedEmail>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationCreatedEmailHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;          
        }

        public async Task Handle(ApplicationCreatedEmail @event, CancellationToken cancellationToken = default)
        {          
            await _eventPublisher.Publish(new ApplicationCreatedEmailEvent(@event.ApplicationId, @event.PledgeId, @event.ReceiverAccountId));
        }
    }
}
