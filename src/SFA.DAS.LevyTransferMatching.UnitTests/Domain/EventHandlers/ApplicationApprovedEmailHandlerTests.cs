using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers
{
    [TestFixture]
    public class ApplicationApprovedEmailHandlerTests
    {
        private ApplicationApprovedEmailHandler _handler;
        private ApplicationApprovedEmail _event;
        private Mock<IEventPublisher> _eventPublisher;
        private readonly Fixture _fixture = new Fixture();
        private Mock<IPledgeRepository> _pledgeRepository;
        private Mock<IApplicationRepository> _applicationRepository;
        private long _transferSenderId;
        private long _transferReceiverId;

        [SetUp]
        public void Setup()
        {
            _event = _fixture.Create<ApplicationApprovedEmail>();

            _transferSenderId = _fixture.Create<long>();
            _transferReceiverId = _fixture.Create<long>();

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationApprovedEmailEvent>()));

            var pledge = new Pledge(EmployerAccount.New(_transferSenderId, "Test Sender"), new CreatePledgeProperties(), UserInfo.System);

            _pledgeRepository = new Mock<IPledgeRepository>();
            _pledgeRepository.Setup(x => x.Get(It.Is<int>(p => p == _event.PledgeId)))
                .ReturnsAsync(() => pledge);

            var applicationProperties = new CreateApplicationProperties();
            applicationProperties.EmailAddresses = new List<string>();

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(It.Is<int>(p => p == _event.ApplicationId), 
                                                    It.IsAny<int?>(), It.IsAny<long?>()))
                .ReturnsAsync(() => new LevyTransferMatching.Data.Models.Application(It.IsAny<Pledge>(), EmployerAccount.New(_transferReceiverId, "Test Receiver"),
                 applicationProperties, UserInfo.System));

            _handler = new ApplicationApprovedEmailHandler(_eventPublisher.Object, _pledgeRepository.Object, _applicationRepository.Object);
        }

        [Test]
        public async Task Handle_Relays_Event()
        {
            await _handler.Handle(_event, CancellationToken.None);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationApprovedEmailEvent>(e =>
                    e.ApplicationId == _event.ApplicationId &&
                    e.PledgeId == _event.PledgeId &&
                    e.TransferSenderId == _transferSenderId &&
                    e.TransferReceiverId == _transferReceiverId &&
                    e.ReceiverAccountId == _event.ReceiverAccountId)));
        }
    }
}
