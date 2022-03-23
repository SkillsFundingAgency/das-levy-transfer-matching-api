using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationFundingDeclinedHandler : IDomainEventHandler<ApplicationFundingDeclined>
    {
        private readonly IEventPublisher _eventPublisher;

        public ApplicationFundingDeclinedHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(ApplicationFundingDeclined @event, CancellationToken cancellationToken = default)
        {
            await _eventPublisher.Publish(
                new ApplicationFundingDeclinedEvent(
                    @event.ApplicationId,
                    @event.PledgeId,
                    @event.DeclinedOn,
                    @event.Amount,
                    @event.AccountId));
        }
    }
}