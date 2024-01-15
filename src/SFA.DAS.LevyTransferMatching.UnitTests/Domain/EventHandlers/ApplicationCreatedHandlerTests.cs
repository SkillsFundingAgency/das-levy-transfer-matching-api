using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers;

[TestFixture]
public class ApplicationCreatedHandlerTests
{
    private ApplicationCreatedHandler _handler;
    private ApplicationCreated _event;
    private Mock<IEventPublisher> _eventPublisher;
    private readonly Fixture _fixture = new();
    private Mock<IPledgeRepository> _pledgeRepository;
    private long _transferSenderId;

    [SetUp]
    public void Setup()
    {
        _event = _fixture.Create<ApplicationCreated>();

        _transferSenderId = _fixture.Create<long>();

        _eventPublisher = new Mock<IEventPublisher>();
        _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationCreatedEvent>()));

        _pledgeRepository = new Mock<IPledgeRepository>();
        _pledgeRepository.Setup(repository => repository.Get(It.Is<int>(p => p == _event.PledgeId)))
            .ReturnsAsync(() => new Pledge(EmployerAccount.New(_transferSenderId, "Test Sender"),
                new CreatePledgeProperties(), UserInfo.System));

        _handler = new ApplicationCreatedHandler(_eventPublisher.Object, _pledgeRepository.Object);
    }

    [Test]
    public async Task Handle_Relays_Event()
    {
        await _handler.Handle(_event, CancellationToken.None);

        _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationCreatedEvent>(applicationCreatedEvent =>
            applicationCreatedEvent.ApplicationId == _event.ApplicationId &&
            applicationCreatedEvent.PledgeId == _event.PledgeId &&
            applicationCreatedEvent.CreatedOn == _event.CreatedOn &&
            applicationCreatedEvent.TransferSenderId == _transferSenderId &&
            applicationCreatedEvent.ReceiverAccountId == _event.ReceiverAccountId)));
    }
}