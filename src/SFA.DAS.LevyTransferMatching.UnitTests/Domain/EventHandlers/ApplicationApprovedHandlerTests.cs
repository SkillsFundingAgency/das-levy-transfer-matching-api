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

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers;

[TestFixture]
public class ApplicationApprovedHandlerTests
{
    private ApplicationApprovedHandler _handler;
    private ApplicationApproved _event;
    private Mock<IEventPublisher> _eventPublisher;
    private readonly Fixture _fixture = new Fixture();
    private Mock<IPledgeRepository> _pledgeRepository;
    private long _transferSenderId;

    [SetUp]
    public void Setup()
    {
        _event = _fixture.Create<ApplicationApproved>();

        _transferSenderId = _fixture.Create<long>();

        _eventPublisher = new Mock<IEventPublisher>();
        _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationApprovedEvent>()));

        _pledgeRepository = new Mock<IPledgeRepository>();
        _pledgeRepository.Setup(x => x.Get(It.Is<int>(p => p == _event.PledgeId)))
            .ReturnsAsync(() => new Pledge(EmployerAccount.New(_transferSenderId, "Test Sender"),
                new CreatePledgeProperties(), UserInfo.System));

        _handler = new ApplicationApprovedHandler(_eventPublisher.Object, _pledgeRepository.Object);
    }

    [Test]
    public async Task Handle_Relays_Event()
    {
        await _handler.Handle(_event, CancellationToken.None);

        _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationApprovedEvent>(e =>
            e.ApplicationId == _event.ApplicationId &&
            e.PledgeId == _event.PledgeId &&
            e.ApprovedOn == _event.ApprovedOn &&
            e.Amount == _event.Amount &&
            e.TransferSenderId == _transferSenderId &&
            e.ReceiverAccountId == _event.ReceiverAccountId)));
    }
}