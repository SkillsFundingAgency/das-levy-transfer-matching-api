using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ExpireAcceptedFunding;

public class ExpireAcceptedFundingCommandHandlerTests
{
    private Fixture _fixture;
    private Mock<IApplicationRepository> _mockApplicationRepository;
    private ExpireAcceptedFundingCommandHandler _expireAcceptedFundingCommandHandler;
    private ExpireAcceptedFundingCommand _request;
    private Data.Models.Application _application;
    private Data.Models.Application _updatedApplication;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();

        _mockApplicationRepository = new Mock<IApplicationRepository>();
        var mockLogger = new Mock<ILogger<ExpireAcceptedFundingCommandHandler>>();

        // Arrange
        _request = _fixture.Create<ExpireAcceptedFundingCommand>();

        var userInfo = _fixture.Create<UserInfo>();
        _application = _fixture.Create<Data.Models.Application>();

        _application.Approve(userInfo);
        _application.AcceptFunding(userInfo);

        _mockApplicationRepository
            .Setup(x => x.Get(It.Is<int>(y => y == _request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == null)))
            .ReturnsAsync(_application);

        Action<Data.Models.Application> updateCallback = x => { _updatedApplication = x; };

        _mockApplicationRepository
            .Setup(x => x.Update(It.Is<Data.Models.Application>(y => y == _application)))
            .Callback(updateCallback);

        _expireAcceptedFundingCommandHandler = new ExpireAcceptedFundingCommandHandler(_mockApplicationRepository.Object, mockLogger.Object);
    }

    [Test]
    public async Task Handle_ApplicationExists_ApplicationUpdatedToExpired()
    {
        // Act
        var result = await _expireAcceptedFundingCommandHandler.Handle(_request, CancellationToken.None);

        // Assert
        result.Updated.Should().BeTrue();
        _updatedApplication.Should().NotBeNull();
        _updatedApplication.Status.Should().Be(ApplicationStatus.FundsExpired);
    }

    [Test]
    public async Task Handle_ApplicationDoesntExist_ApplicationNotUpdated()
    {
        // Arrange
        _mockApplicationRepository
            .Setup(x => x.Get(It.Is<int>(y => y == _request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == null)))
            .ReturnsAsync((Data.Models.Application)null);

        // Act
        var result = await _expireAcceptedFundingCommandHandler.Handle(_request, CancellationToken.None);

        // Assert
        result.Updated.Should().BeFalse();
    }

    [Test]
    public async Task Handle_When_Application_Under_New_Cost_Model_Amount_Is_Correct()
    {
        _application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);

        await _expireAcceptedFundingCommandHandler.Handle(_request, CancellationToken.None);

        var events = _application.FlushEvents();
        events.Any(x => x is ApplicationFundingExpired approvalEvent && approvalEvent.ApplicationId == _application.Id).Should().BeTrue();
    }
}