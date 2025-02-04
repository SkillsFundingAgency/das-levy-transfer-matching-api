using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Api.Models.GetApplication;
using SFA.DAS.LevyTransferMatching.Api.Models.GetApplicationsToAutoExpire;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoExpire;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers;

[TestFixture]
public class ApplicationsControllerTests
{
    private readonly Fixture _fixture = new Fixture();
    private Mock<IMediator> _mediator;
    private Mock<ILogger<ApplicationsController>> _logger;
    private ApplicationsController _applicationsController;

    private int _pledgeId;
    private int _applicationId;
    private long _accountId;
    private long _senderAccountId;
    private CreateApplicationRequest _request;
    private CreateApplicationCommandResult _result;
    private DebitApplicationRequest _debitApplicationRequest;
    private WithdrawApplicationRequest _withdrawApplicationRequest;
    private DeclineFundingRequest _declineFundingRequest;
    private ExpireAcceptedFundingRequest _expireAcceptedFundingRequest;

    [SetUp]
    public void Setup()
    {
        _pledgeId = _fixture.Create<int>();
        _applicationId = _fixture.Create<int>();
        _accountId = _fixture.Create<long>();
        _senderAccountId = _fixture.Create<long>();
        _request = _fixture.Create<CreateApplicationRequest>();
        _result = _fixture.Create<CreateApplicationCommandResult>();
        _debitApplicationRequest = _fixture.Create<DebitApplicationRequest>();
        _withdrawApplicationRequest = _fixture.Create<WithdrawApplicationRequest>();
        _expireAcceptedFundingRequest = _fixture.Create<ExpireAcceptedFundingRequest>();
        _declineFundingRequest = _fixture.Create<DeclineFundingRequest>();

        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<ApplicationsController>>();
        _applicationsController = new ApplicationsController(_mediator.Object, _logger.Object);

        _mediator.Setup(x => x.Send(It.Is<CreateApplicationCommand>(command =>
                command.PledgeId == _pledgeId &&
                command.EmployerAccountId == _request.EmployerAccountId &&
                command.Details == _request.Details &&
                command.StandardId == _request.StandardId &&
                command.StandardTitle == _request.StandardTitle &&
                command.StandardLevel == _request.StandardLevel &&
                command.StandardDuration == _request.StandardDuration &&
                command.StandardMaxFunding == _request.StandardMaxFunding &&
                command.StandardRoute == _request.StandardRoute &&
                command.NumberOfApprentices == _request.NumberOfApprentices &&
                command.StartDate == _request.StartDate &&
                command.HasTrainingProvider == _request.HasTrainingProvider &&
                command.Sectors.Equals(_request.Sectors) &&
                command.Locations == _request.Locations &&
                command.AdditionalLocation == _request.AdditionalLocation &&
                command.SpecificLocation == _request.SpecificLocation &&
                command.FirstName == _request.FirstName &&
                command.LastName == _request.LastName &&
                command.EmailAddresses.Equals(_request.EmailAddresses) &&
                command.BusinessWebsite == _request.BusinessWebsite &&
                command.UserId == _request.UserId &&
                command.UserDisplayName == _request.UserDisplayName
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_result);
    }

    [Test]
    public async Task Post_ApproveApplication_Approves_Application()
    {
        var request = _fixture.Create<ApproveApplicationRequest>();

        var actionResult = await _applicationsController.ApproveApplication(_pledgeId, _applicationId, request);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<ApproveApplicationCommand>(command =>
                    command.PledgeId == _pledgeId &&
                    command.ApplicationId == _applicationId &&
                    command.UserId == request.UserId &&
                    command.UserDisplayName == request.UserDisplayName),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Post_UndoApplicationApproval_Undoes_Application_Approval()
    {
        var actionResult = await _applicationsController.UndoApplicationApproval(_pledgeId, _applicationId);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<UndoApplicationApprovalCommand>(command =>
                    command.PledgeId == _pledgeId &&
                    command.ApplicationId == _applicationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Post_Returns_ApplicationId()
    {
        var actionResult = await _applicationsController.CreateApplication(_pledgeId, _request);
        var createdResult = actionResult as CreatedResult;
        createdResult.Should().NotBeNull();
        var response = createdResult?.Value as CreateApplicationResponse;
        response.Should().NotBeNull();
        response?.ApplicationId.Should().Be(_result.ApplicationId);
    }

    [Test]
    public async Task Get_Returns_Application()
    {
        // Arrange
        var pledgeId = (int?)null;
        var applicationId = _fixture.Create<int>();
        var applicationResult = _fixture.Create<GetApplicationResult>();

        _mediator
            .Setup(x => x.Send(
                It.Is<GetApplicationQuery>(y => y.PledgeId == pledgeId && y.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicationResult);

        // Act
        var actionResult = await _applicationsController.GetApplication(applicationId);
        var okObjectResult = actionResult as OkObjectResult;
        var getApplicationResponse = okObjectResult?.Value as GetApplicationResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        getApplicationResponse.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task GetApplicationsToAutoDecline_Returns_Applications()
    {
        // Arrange
        var applicationResult = _fixture.Create<GetApplicationsToAutoDeclineResult>();

        _mediator.Setup(x => x.Send(It.IsAny<GetApplicationsToAutoDeclineQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(applicationResult);

        // Act
        var actionResult = await _applicationsController.GetApplicationsToAutoDecline();
        var okObjectResult = actionResult as OkObjectResult;
        var getApplicationResponse = okObjectResult?.Value as GetApplicationsToAutoDeclineResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        getApplicationResponse.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task Get_With_PledgeId_Returns_Application()
    {
        // Arrange
        var pledgeId = _fixture.Create<int>();
        var applicationId = _fixture.Create<int>();
        var applicationResult = _fixture.Create<GetApplicationResult>();

        _mediator
            .Setup(x => x.Send(
                It.Is<GetApplicationQuery>(y => y.PledgeId == pledgeId && y.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(applicationResult);

        // Act
        var actionResult = await _applicationsController.GetApplication(pledgeId, applicationId);
        var okObjectResult = actionResult as OkObjectResult;
        var getApplicationResponse = okObjectResult?.Value as GetApplicationResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        getApplicationResponse.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task Get_With_PledgeId_Returns_NotFound()
    {
        // Arrange
        var pledgeId = _fixture.Create<int>();
        var applicationId = _fixture.Create<int>();

        _mediator
            .Setup(x => x.Send(
                It.Is<GetApplicationQuery>(y => y.PledgeId == pledgeId && y.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetApplicationResult)null);

        // Act
        var actionResult = await _applicationsController.GetApplication(pledgeId, applicationId);
        var okObjectResult = actionResult as NotFoundResult;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Returns_NotFound()
    {
        // Arrange
        var pledgeId = (int?)null;
        var applicationId = _fixture.Create<int>();

        _mediator
            .Setup(x => x.Send(
                It.Is<GetApplicationQuery>(y => y.PledgeId == pledgeId && y.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetApplicationResult)null);

        // Act
        var actionResult = await _applicationsController.GetApplication(applicationId);
        var okObjectResult = actionResult as NotFoundResult;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Returns_Applications()
    {
        _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(query => query.PledgeId == _pledgeId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationsResult
            { Items = [new GetApplicationsResult.Application()] });

        var actionResult =
            await _applicationsController.GetApplications(new GetApplicationsRequest { PledgeId = _pledgeId });
        var result = actionResult as OkObjectResult;
        result.Should().NotBeNull();
        var response = result?.Value as GetApplicationsResponse;
        response.Should().NotBeNull();
        response?.Applications.Count().Should().Be(1);
    }

    [Test]
    public async Task Get_Returns_Applications_By_Account_Id()
    {
        _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(query => query.AccountId == _accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationsResult
            { Items = [new GetApplicationsResult.Application()] });

        var actionResult = await _applicationsController.GetApplications(new GetApplicationsRequest { AccountId = _accountId });
        var result = actionResult as OkObjectResult;
        result.Should().NotBeNull();
        var response = result?.Value as GetApplicationsResponse;
        response.Should().NotBeNull();

        response?.Applications.Count().Should().Be(1);
    }

    [Test]
    public async Task Get_Returns_Applications_By_SenderAccount_Id()
    {
        _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(query => query.SenderAccountId == _senderAccountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationsResult
            { Items = [new GetApplicationsResult.Application()] });

        var actionResult = await _applicationsController.GetApplications(new GetApplicationsRequest
        { SenderAccountId = _senderAccountId });
        var result = actionResult as OkObjectResult;
        result.Should().NotBeNull();
        var response = result?.Value as GetApplicationsResponse;
        response.Should().NotBeNull();

        response?.Applications.Count().Should().Be(1);
    }


    [Test]
    public async Task Get_Returns_Not_Empty_Applications_By_Status()
    {
        var applicationStatusFilter = _fixture.Create<Data.Enums.ApplicationStatus>();
        _mediator.Setup(x =>
                x.Send(It.Is<GetApplicationsQuery>(query => query.ApplicationStatusFilter == applicationStatusFilter),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationsResult
            { Items = [new GetApplicationsResult.Application()] });

        var actionResult = await _applicationsController.GetApplications(new GetApplicationsRequest
        { ApplicationStatusFilter = applicationStatusFilter });
        var result = actionResult as OkObjectResult;
        result.Should().NotBeNull();
        var response = result?.Value as GetApplicationsResponse;
        response.Should().NotBeNull();

        response?.Applications.Count().Should().Be(1);
    }

    [Test]
    public async Task Post_AcceptFunding_Returns_No_Content()
    {
        // Arrange
        var applicationId = _fixture.Create<int>();
        var accountId = _fixture.Create<long>();
        var request = _fixture.Create<AcceptFundingRequest>();
        var result = new AcceptFundingCommandResult()
        {
            Updated = true,
        };

        _mediator
            .Setup(x => x.Send(It.IsAny<AcceptFundingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _applicationsController.AcceptFunding(applicationId, accountId, request);
        var noContentResult = actionResult as NoContentResult;

        // Assert
        actionResult.Should().NotBeNull();
        noContentResult.Should().NotBeNull();
    }

    [Test]
    public async Task Post_AcceptFunding_Returns_BadRequest()
    {
        // Arrange
        var applicationId = _fixture.Create<int>();
        var accountId = _fixture.Create<long>();
        var request = _fixture.Create<AcceptFundingRequest>();
        var result = new AcceptFundingCommandResult()
        {
            Updated = false,
        };

        _mediator
            .Setup(x => x.Send(It.IsAny<AcceptFundingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _applicationsController.AcceptFunding(applicationId, accountId, request);
        var badRequestResult = actionResult as BadRequestResult;

        // Assert
        actionResult.Should().NotBeNull();
        badRequestResult.Should().NotBeNull();
    }

    [Test]
    public async Task Post_DebitApplication_Debits_Application()
    {
        var actionResult = await _applicationsController.DebitApplication(_applicationId, _debitApplicationRequest);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<DebitApplicationCommand>(command =>
                    command.ApplicationId == _applicationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Post_WithdrawApplication_Withdraws_Application()
    {
        var actionResult =
            await _applicationsController.WithdrawApplication(_applicationId, _accountId, _withdrawApplicationRequest);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<WithdrawApplicationCommand>(command =>
                    command.ApplicationId == _applicationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Post_DeclineApprovedFunding_Declines_Application()
    {
        var result = new DeclineFundingCommandResult()
        {
            Updated = true,
        };

        _mediator
            .Setup(x => x.Send(It.IsAny<DeclineFundingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actionResult =
            await _applicationsController.DeclineApprovedFunding(_applicationId, _declineFundingRequest);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<DeclineFundingCommand>(command =>
                    command.ApplicationId == _applicationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetApplicationsToAutoExpire_Returns_Applications()
    {
        // Arrange
        var applicationResult = _fixture.Create<GetApplicationsToAutoExpireResult>();

        _mediator.Setup(x => x.Send(It.IsAny<GetApplicationsToAutoExpireQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(applicationResult);

        // Act
        var actionResult = await _applicationsController.GetApplicationsToAutoExpire();
        var okObjectResult = actionResult as OkObjectResult;
        var getApplicationResponse = okObjectResult?.Value as GetApplicationsToAutoExpireResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        getApplicationResponse.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task Post_ExpireApprovedFunding_Expires_Application()
    {
        var result = new ExpireAcceptedFundingCommandResult()
        {
            Updated = true,
        };

        _mediator
            .Setup(x => x.Send(It.IsAny<ExpireAcceptedFundingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actionResult =
            await _applicationsController.ExpireAcceptedFunding(_applicationId, _expireAcceptedFundingRequest);
        var okResult = actionResult as OkResult;
        okResult.Should().NotBeNull();

        _mediator.Verify(x => x.Send(It.Is<ExpireAcceptedFundingCommand>(command =>
                    command.ApplicationId == _applicationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}