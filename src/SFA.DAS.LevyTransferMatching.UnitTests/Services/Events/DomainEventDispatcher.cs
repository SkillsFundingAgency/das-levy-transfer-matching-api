using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services.Events;

public class WhenSendIsCalled
{
    private LevyTransferMatching.Services.Events.DomainEventDispatcher _domainEventDispatcher;
    private Mock<IServiceProvider> _mockServiceProvider;
    private Mock<IDomainEventHandler<TestEvent>> _mockTestEventHandler;
    private Mock<IDomainEventHandler<TestEvent>> _mockTestEvent2Handler;

    private Fixture _fixture;

    public class TestEvent : IDomainEvent;

    public class TestEvent2 : IDomainEvent;

    [SetUp]
    public void Arrange()
    {
        _fixture = new Fixture();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockTestEventHandler = new Mock<IDomainEventHandler<TestEvent>>();
        _mockTestEvent2Handler = new Mock<IDomainEventHandler<TestEvent>>();

        var eventHandlers = new List<IDomainEventHandler<TestEvent>>
        {
            _mockTestEventHandler.Object,
            _mockTestEvent2Handler.Object
        };

        _mockServiceProvider.Setup(m => m.GetService(typeof(IEnumerable<IDomainEventHandler<TestEvent>>)))
            .Returns(eventHandlers);

        _mockServiceProvider.Setup(m => m.GetService(typeof(IEnumerable<IDomainEventHandler<TestEvent2>>)))
            .Returns(null!);

        _domainEventDispatcher = new LevyTransferMatching.Services.Events.DomainEventDispatcher(_mockServiceProvider.Object);
    }

    [Test]
    public async Task Then_the_matching_domain_event_handlers_are_retrieved_from_the_service_provider()
    {
        //Arrange
        var domainEvent = _fixture.Create<TestEvent>();

        //Act
        await _domainEventDispatcher.Send(domainEvent);

        //Assert
        _mockServiceProvider.Verify(m => m.GetService(typeof(IEnumerable<IDomainEventHandler<TestEvent>>)), Times.Exactly(1));
    }

    [Test]
    public async Task Then_the_event_is_handled_by_the_matching_handlers()
    {
        //Arrange
        var domainEvent = _fixture.Create<TestEvent>();
        var isHandled1 = false;
        var isHandled2 = false;

        _mockTestEventHandler.Setup(m => m.Handle(domainEvent, It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                isHandled1 = true;
            });

        _mockTestEvent2Handler.Setup(m => m.Handle(domainEvent, It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                isHandled2 = true;
            });

        //Act
        await _domainEventDispatcher.Send(domainEvent);

        //Assert
        isHandled1.Should().BeTrue();
        isHandled2.Should().BeTrue();
    }

    [Test]
    public async Task Then_no_exceptions_are_raised_if_there_is_no_matching_handler()
    {
        // Arrange
        var domainEvent = _fixture.Create<TestEvent2>();

        _mockServiceProvider.Setup(m => m.GetService(typeof(IEnumerable<IDomainEventHandler<TestEvent2>>)))
            .Returns(new List<IDomainEventHandler<TestEvent2>>());

        // Act
        var action = () => _domainEventDispatcher.Send(domainEvent);

        // Assert
        await action.Should().NotThrowAsync();
    }
}