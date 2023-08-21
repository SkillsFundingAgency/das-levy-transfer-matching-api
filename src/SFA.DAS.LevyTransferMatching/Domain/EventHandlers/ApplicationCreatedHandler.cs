using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.Domain.EventHandlers
{
    public class ApplicationCreatedHandler : IDomainEventHandler<ApplicationCreated>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly ILogger<ApplicationCreatedHandler> _logger;

        public ApplicationCreatedHandler(IEventPublisher eventPublisher, IPledgeRepository pledgeRepository, ILogger<ApplicationCreatedHandler> logger)
        {
            _eventPublisher = eventPublisher;
            _pledgeRepository = pledgeRepository;
            _logger = logger;
        }

        public async Task Handle(ApplicationCreated @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{TypeName} started processing application with id: {Id}.", nameof(ApplicationCreatedHandler), @event.ApplicationId);
            
            var pledge = await _pledgeRepository.Get(@event.PledgeId);

            var senderId = pledge.EmployerAccountId;

            await _eventPublisher.Publish(new ApplicationCreatedEvent(@event.ApplicationId, @event.PledgeId,
                @event.CreatedOn,
                senderId));
            
            _logger.LogInformation("{TypeName} completed processing.", nameof(ApplicationCreatedHandler));
        }
    }
}
