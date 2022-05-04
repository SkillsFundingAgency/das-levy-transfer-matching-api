using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers
{
    public class ApplicationWithdrawnAfterAcceptanceHandlerTests
    {
        private ApplicationWithdrawnAfterAcceptanceHandler _handler;
        private ApplicationWithdrawnAfterAcceptance _event;
        private Mock<IEventPublisher> _eventPublisher;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _event = _fixture.Create<ApplicationWithdrawnAfterAcceptance>();

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationWithdrawnAfterAcceptanceEvent>()));

            _handler = new ApplicationWithdrawnAfterAcceptanceHandler(_eventPublisher.Object);
        }

        [Test]
        public async Task Handle_Relays_Event()
        {
            await _handler.Handle(_event, CancellationToken.None);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationWithdrawnAfterAcceptanceEvent>(e =>
                e.ApplicationId == _event.ApplicationId &&
                e.PledgeId == _event.PledgeId &&
                e.Amount == _event.Amount)));
        }
    }
}
