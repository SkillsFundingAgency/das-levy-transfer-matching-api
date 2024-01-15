using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Messages.Events;
using SFA.DAS.NServiceBus.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.EventHandlers;

[TestFixture]
public class PledgeClosedEventHandlerTests
{
    private PledgeClosedEventHandler _handler;
    private Mock<IEventPublisher> _eventPublisher;
    private readonly Fixture _fixture = new();

    [SetUp]
    public void Setup()
    {
        _eventPublisher = new Mock<IEventPublisher>();
        _eventPublisher.Setup(x => x.Publish(It.IsAny<PledgeClosedEvent>()));
        _handler = new PledgeClosedEventHandler(_eventPublisher.Object);
    }

    [Test]
    public async Task Handle_Relays_Event()
    {
        var @event = _fixture.Create<PledgeClosed>();

        await _handler.Handle(@event, CancellationToken.None);

        _eventPublisher.Verify(x => x.Publish(It.Is<PledgeClosedEvent>(pledgeClosedEvent =>
            pledgeClosedEvent.PledgeId == @event.PledgeId &&
            pledgeClosedEvent.InsufficientFunds == @event.InsufficientFunds)));
    }
}