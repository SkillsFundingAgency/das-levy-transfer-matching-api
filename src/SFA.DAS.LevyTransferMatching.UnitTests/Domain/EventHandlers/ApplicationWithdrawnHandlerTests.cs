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
public class ApplicationWithdrawnHandlerTests
{
    private ApplicationWithdrawnHandler _handler;
    private ApplicationWithdrawn _event;
    private Mock<IEventPublisher> _eventPublisher;
    private readonly Fixture _fixture = new Fixture();
    private Mock<IPledgeRepository> _pledgeRepository;
    private long _transferSenderId;

    [SetUp]
    public void Setup()
    {
        _event = _fixture.Create<ApplicationWithdrawn>();

        _transferSenderId = _fixture.Create<long>();

        _eventPublisher = new Mock<IEventPublisher>();
        _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationWithdrawnEvent>()));

        _pledgeRepository = new Mock<IPledgeRepository>();
        _pledgeRepository.Setup(x => x.Get(It.Is<int>(p => p == _event.PledgeId)))
            .ReturnsAsync(() => new Pledge(EmployerAccount.New(_transferSenderId, "Test Sender"),
                new CreatePledgeProperties(), UserInfo.System));

        _handler = new ApplicationWithdrawnHandler(_eventPublisher.Object, _pledgeRepository.Object);
    }

    [Test]
    public async Task Handle_Relays_Event()
    {
        await _handler.Handle(_event, CancellationToken.None);

        _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationWithdrawnEvent>(e =>
            e.ApplicationId == _event.ApplicationId &&
            e.PledgeId == _event.PledgeId &&
            e.WithdrawnOn == _event.WithdrawnOn &&
            e.TransferSenderId == _transferSenderId)));
    }
}