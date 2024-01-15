using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DeclineFunding;

public class DeclineFundingCommandHandlerTests
{
    private Fixture _fixture;
    private Mock<IApplicationRepository> _mockApplicationRepository;
    private DeclineFundingCommandHandler _declineFundingCommandHandler;
    private DeclineFundingCommand _request;
    private Data.Models.Application _application;
    private Data.Models.Application _updatedApplication;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();

        _mockApplicationRepository = new Mock<IApplicationRepository>();
        var mockLogger = new Mock<ILogger<DeclineFundingCommandHandler>>();

        // Arrange
        _request = _fixture.Create<DeclineFundingCommand>();

        var userInfo = _fixture.Create<UserInfo>();
        _application = _fixture.Create<Data.Models.Application>();
        //_application.SetValue(x => x.Amount, _fixture.Create<int>());

        // Make it 'approved', otherwise declining will fail
        _application.Approve(userInfo);

        _mockApplicationRepository
            .Setup(x => x.Get(It.Is<int>(y => y == _request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == _request.AccountId)))
            .ReturnsAsync(_application);

        Action<Data.Models.Application> updateCallback =
            (x) =>
            {
                _updatedApplication = x;
            };

        _mockApplicationRepository
            .Setup(x => x.Update(It.Is<Data.Models.Application>(y => y == _application)))
            .Callback(updateCallback);

        _declineFundingCommandHandler = new DeclineFundingCommandHandler(_mockApplicationRepository.Object, mockLogger.Object);
    }

    [Test]
    public async Task Handle_ApplicationExists_ApplicationUpdatedToDeclined()
    {
        // Act
        var result = await _declineFundingCommandHandler.Handle(_request, CancellationToken.None);

        // Assert
        Assert.That(result.Updated, Is.True);
        Assert.That(_updatedApplication, Is.Not.Null);
        Assert.That(_updatedApplication.Status, Is.EqualTo(ApplicationStatus.Declined));
    }

    [Test]
    public async Task Handle_ApplicationDoesntExist_ApplicationNotUpdated()
    {
        // Arrange
        _mockApplicationRepository
            .Setup(x => x.Get(It.Is<int>(y => y == _request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == _request.AccountId)))
            .ReturnsAsync((LevyTransferMatching.Data.Models.Application)null);

        // Act
        var result = await _declineFundingCommandHandler.Handle(_request, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.Updated);
    }

    [Test]
    public async Task Handle_When_Application_Under_New_Cost_Model_Amount_Is_Correct()
    {
        _application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);

        await _declineFundingCommandHandler.Handle(_request, CancellationToken.None);

        var events = _application.FlushEvents();
        Assert.That(events.Any(x => x is ApplicationFundingDeclined approvalEvent && approvalEvent.Amount == _application.GetCost()), Is.True);
    }
}