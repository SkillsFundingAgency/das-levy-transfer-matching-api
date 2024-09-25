using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.CreatePledge;
using SFA.DAS.LevyTransferMatching.Api.Models.GetPledge;
using SFA.DAS.LevyTransferMatching.Api.Models.GetPledges;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers;

[TestFixture]
public class PledgesControllerTests
{
    private Fixture _fixture;
    private Mock<IMediator> _mockMediator;
    private PledgesController _pledgesController;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _mockMediator = new Mock<IMediator>();
        _pledgesController = new PledgesController(_mockMediator.Object);
    }

    [TearDown]
    public void TearDown() => _pledgesController?.Dispose();

    [Test]
    public async Task POST_Create_Returns_Created_With_Correct_Location()
    {
        // Arrange 
        var accountId = _fixture.Create<long>();
        var request = _fixture.Create<CreatePledgeRequest>();
        var result = _fixture.Create<CreatePledgeResult>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CreatePledgeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _pledgesController.CreatePledge(accountId, request);
        var createdResult = actionResult as CreatedResult;
        var createPledgeResponse = createdResult?.Value as CreatePledgeResponse;

        // Assert
        actionResult.Should().NotBeNull();
        createdResult.Should().NotBeNull();
        createPledgeResponse.Should().NotBeNull();
        createdResult?.StatusCode.Should().Be((int)HttpStatusCode.Created);
        $"/accounts/{accountId}/pledges/{result.Id}".Should().Be(createdResult?.Location);
        result.Id.Should().Be(createPledgeResponse!.Id);
    }

    [Test]
    public void POST_Create_Throws_Validation_Error()
    {
        // Arrange 
        var accountId = _fixture.Create<long>();
        var request = _fixture.Create<CreatePledgeRequest>();
        var validationException = _fixture.Create<ValidationException>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<CreatePledgeCommand>(), It.IsAny<CancellationToken>()))
            .Throws(validationException);

        // Assert
        var action = () => _pledgesController.CreatePledge(accountId, request);
        action.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task GET_Pledge_Returns_Requested_Record()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var pledgeResult = _fixture.Create<GetPledgeResult>();

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgeQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pledgeResult);

        // Act
        var actionResult = await _pledgesController.GetPledge(id);
        var okObjectResult = actionResult as OkObjectResult;
        var getPledgeResponse = okObjectResult?.Value as GetPledgeResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        getPledgeResponse.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        pledgeResult.Id.Should().Be(getPledgeResponse!.Id);
    }

    [Test]
    public async Task GET_Pledge_Requested_Doesnt_Exist_NotFound_Returned()
    {
        // Arrange
        var id = _fixture.Create<int>();

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgeQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPledgeResult)null);

        // Act
        var actionResult = await _pledgesController.GetPledge(id);
        var notFoundResult = actionResult as NotFoundResult;

        // Assert
        actionResult.Should().NotBeNull();
        notFoundResult.Should().NotBeNull();
        notFoundResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test]
    public async Task POST_Debit_Debits_Pledge_By_Requested_Amount()
    {
        var pledgeId = _fixture.Create<int>();
        var request = _fixture.Create<DebitPledgeRequest>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<DebitPledgeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new DebitPledgeCommandResult { IsSuccess = true });

        await _pledgesController.DebitPledge(pledgeId, request);

        _mockMediator.Verify(x =>
            x.Send(It.Is<DebitPledgeCommand>(command =>
                    command.PledgeId == pledgeId && command.Amount == request.Amount && command.ApplicationId == request.ApplicationId),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task POST_Debit_Credits_Pledge_By_Request_Amount()
    {
        var pledgeId = _fixture.Create<int>();
        var request = _fixture.Create<CreditPledgeRequest>();

        await _pledgesController.CreditPledge(pledgeId, request);

        _mockMediator.Verify(x =>
            x.Send(It.Is<CreditPledgeCommand>(command =>
                    command.PledgeId == pledgeId && command.Amount == request.Amount && command.ApplicationId == request.ApplicationId),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task POST_Close_A_Pledge_By_PledgeId()
    {
        var pledgeId = _fixture.Create<int>();
        var request = _fixture.Create<ClosePledgeRequest>();

        var actionResult = await _pledgesController.ClosePledge(pledgeId, request);
        var okResult = actionResult as OkResult;

        actionResult.Should().NotBeNull();
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task POST_Close_A_Pledge_By_PledgeId_Returns_NotFound()
    {
        var pledgeId = _fixture.Create<int>();
        var request = _fixture.Create<ClosePledgeRequest>();
        var aggregateNotFoundException = _fixture.Create<AggregateNotFoundException<Pledge>>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<ClosePledgeCommand>(), It.IsAny<CancellationToken>()))
            .Throws(aggregateNotFoundException);

        var actionResult = await _pledgesController.ClosePledge(pledgeId, request);

        actionResult.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task GET_All_Pledges_Returned()
    {
        // Arrange
        var expectedPledges = _fixture.CreateMany<GetPledgesResult.Pledge>().ToList();

        var result = new GetPledgesResult()
        {
            Items = expectedPledges.ToList(),
            TotalItems = expectedPledges.Count,
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgesQuery>(x => x.AccountId == null), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _pledgesController.GetPledges();
        var okObjectResult = actionResult as OkObjectResult;
        var response = okObjectResult?.Value as GetPledgesResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        response.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);

        response?.Pledges.Count().Should().Be(expectedPledges.Count);
        response?.TotalPledges.Should().Be(expectedPledges.Count);
    }

    [Test]
    public async Task GET_Account_Pledges_Returned()
    {
        // Arrange
        var accountId = _fixture.Create<long>();
        var expectedPledges = _fixture.CreateMany<GetPledgesResult.Pledge>().ToArray();
        var sectors = _fixture.Create<IEnumerable<Sector>>();

        var result = new GetPledgesResult
        {
            Items = expectedPledges.ToList(),
            TotalItems = expectedPledges.Length,
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgesQuery>(x => x.AccountId == accountId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _pledgesController.GetPledges(sectors, accountId: accountId);
        var okObjectResult = actionResult as OkObjectResult;
        var response = okObjectResult?.Value as GetPledgesResponse;

        // Assert
        actionResult.Should().NotBeNull();
        okObjectResult.Should().NotBeNull();
        response.Should().NotBeNull();
        okObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);

        response?.Pledges.Count().Should().Be(expectedPledges.Length);
        response?.TotalPledges.Should().Be(expectedPledges.Length);
    }
}