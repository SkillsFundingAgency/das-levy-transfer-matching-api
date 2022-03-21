using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers
{
    [TestFixture]
    public class ApplicationFundingAcceptedHandlerTests
    {
        private ApplicationFundingAcceptedHandler _handler;
        private Mock<IEventPublisher> _eventPublisher;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationFundingAcceptedEvent>()));

            _handler = new ApplicationFundingAcceptedHandler(_eventPublisher.Object);
        }

        [Test]
        public async Task Handle_Relays_Event()
        {
            var @event = _fixture.Create<ApplicationFundingAccepted>();

            await _handler.Handle(@event, CancellationToken.None);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationFundingAcceptedEvent>(e =>
                e.ApplicationId == @event.ApplicationId &&
                e.PledgeId == @event.PledgeId &&
                e.AcceptedOn == @event.AcceptedOn)));
        }
    }
}
