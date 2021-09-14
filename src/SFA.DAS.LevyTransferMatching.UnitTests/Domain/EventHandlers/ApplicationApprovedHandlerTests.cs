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
    public class ApplicationApprovedHandlerTests
    {
        private ApplicationApprovedHandler _handler;
        private Mock<IEventPublisher> _eventPublisher;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationApprovedEvent>()));
            
            _handler = new ApplicationApprovedHandler(_eventPublisher.Object);
        }

        [Test]
        public async Task Handle_Relays_Event()
        {
            var @event = _fixture.Create<ApplicationApproved>();

            await _handler.Handle(@event, CancellationToken.None);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationApprovedEvent>(e =>
                    e.ApplicationId == @event.ApplicationId &&
                    e.PledgeId == @event.PledgeId &&
                    e.ApprovedOn == @event.ApprovedOn &&
                    e.Amount == @event.Amount)));
        }
    }
}
