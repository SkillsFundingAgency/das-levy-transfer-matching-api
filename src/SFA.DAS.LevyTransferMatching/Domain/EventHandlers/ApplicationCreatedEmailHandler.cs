using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationCreatedEmailHandler : IDomainEventHandler<ApplicationCreatedEmail>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationCreatedEmailHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository, IApplicationRepository applicationRepository)
        {
            _eventPublisher = eventPublisher;
            _pledgeRepository = pledgeRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task Handle(ApplicationCreatedEmail @event, CancellationToken cancellationToken = default)
        {
            var pledge = await _pledgeRepository.Get(@event.PledgeId);
            var application = await _applicationRepository.Get(@event.ApplicationId);

            var senderId = pledge.EmployerAccountId;
            var receiverId = application.EmployerAccount.Id;

            await _eventPublisher.Publish(new ApplicationCreatedEmailEvent(@event.ApplicationId, @event.PledgeId, senderId, receiverId, @event.ReceiverAccountId));
        }
    }
}
