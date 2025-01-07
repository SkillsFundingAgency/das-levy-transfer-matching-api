using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers;

[TestFixture]
public class ApplicationFundingExpiredHandlerTests
{
    private ApplicationFundingExpiredHandler _handler;
    private ApplicationFundingExpired _event;
    private Mock<IEventPublisher> _eventPublisher;
    private readonly Fixture _fixture = new();

    [SetUp]
    public void Setup()
    {
        _event = _fixture.Create<ApplicationFundingExpired>();
        _eventPublisher = new Mock<IEventPublisher>();
        _eventPublisher.Setup(x => x.Publish(It.IsAny<ApplicationFundingExpiredEvent>()));
        _handler = new ApplicationFundingExpiredHandler(_eventPublisher.Object);
    }

    [Test]
    public async Task Handle_Relays_Event()
    {
        await _handler.Handle(_event, CancellationToken.None);

        _eventPublisher.Verify(x => x.Publish(It.Is<ApplicationFundingExpiredEvent>(e =>
                e.ApplicationId == _event.ApplicationId
               )));
    }
}
