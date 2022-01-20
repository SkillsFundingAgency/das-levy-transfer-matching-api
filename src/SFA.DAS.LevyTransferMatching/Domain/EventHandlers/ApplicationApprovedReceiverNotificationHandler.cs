using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationApprovedReceiverNotificationHandler : IDomainEventHandler<ApplicationApprovedReceiverNotification>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationApprovedReceiverNotificationHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository, IApplicationRepository applicationRepository)
        {
            _eventPublisher = eventPublisher;
            _pledgeRepository = pledgeRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task Handle(ApplicationApprovedReceiverNotification @event, CancellationToken cancellationToken = default)
        {
            var pledgeTask = _pledgeRepository.Get(@event.PledgeId);

            var applicationTask = _applicationRepository.Get(@event.ApplicationId);

            await Task.WhenAll(pledgeTask, applicationTask);

            var senderId = pledgeTask.Result.EmployerAccountId;

            var receiverId = applicationTask.Result.EmployerAccount.Id;

            await _eventPublisher.Publish(new ApplicationApprovedReceiverNotificationEvent(@event.ApplicationId, @event.PledgeId, senderId, receiverId, @event.ReceiverAccountId));
        }
    }
}
