using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers
{
    [TestFixture]
    public class ApplicationCreatedEmailHandlerTests
    {
        private ApplicationCreatedEmailHandler _handler;
        private ApplicationCreatedEmail _event;
        private Mock<IEventPublisher> _eventPublisher;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _event = _fixture.Create<ApplicationCreatedEmail>();     
            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationCreatedEmailEvent>()));
            _handler = new ApplicationCreatedEmailHandler(_eventPublisher.Object);
        }

        [Test]
        public async Task Handle_Relays_Event()
        {
            await _handler.Handle(_event, CancellationToken.None);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationCreatedEmailEvent>(e =>
                    e.ApplicationId == _event.ApplicationId &&
                    e.PledgeId == _event.PledgeId &&
                    e.ReceiverAccountId == _event.ReceiverAccountId)));
        }
    }
}
