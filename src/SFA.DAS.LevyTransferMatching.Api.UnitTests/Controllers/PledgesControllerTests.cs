﻿using FluentValidation;
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
        var createPledgeResponse = createdResult.Value as CreatePledgeResponse;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createPledgeResponse, Is.Not.Null);
        Assert.That(createdResult.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        Assert.That($"/accounts/{accountId}/pledges/{result.Id}", Is.EqualTo(createdResult.Location));
        Assert.That(result.Id, Is.EqualTo(createPledgeResponse.Id));
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
        Assert.ThrowsAsync<ValidationException>(async () =>
        {
            // Act
            await _pledgesController.CreatePledge(accountId, request);
        });
    }

    [Test]
    public async Task GET_Pledge_Returns_Requested_Record()
    {
        // Arrange
        var id = _fixture.Create<int>();
        var pledgeResult = _fixture.Create<GetPledgeResult>();

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgeQuery>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pledgeResult);

        // Act
        var actionResult = await _pledgesController.GetPledge(id);
        var okObjectResult = actionResult as OkObjectResult;
        var getPledgeResponse = okObjectResult.Value as GetPledgeResponse;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(getPledgeResponse, Is.Not.Null);
        Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(pledgeResult.Id, Is.EqualTo(getPledgeResponse.Id));
    }

    [Test]
    public async Task GET_Pledge_Requested_Doesnt_Exist_NotFound_Returned()
    {
        // Arrange
        var id = _fixture.Create<int>();

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgeQuery>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPledgeResult)null);

        // Act
        var actionResult = await _pledgesController.GetPledge(id);
        var notFoundResult = actionResult as NotFoundResult;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
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

        Assert.That(actionResult, Is.Not.Null);
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
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

        Assert.That(actionResult, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GET_All_Pledges_Returned()
    {
        // Arrange
        var expectedPledges = _fixture.CreateMany<GetPledgesResult.Pledge>();

        var result = new GetPledgesResult()
        {
            Items = expectedPledges.ToList(),
            TotalItems = expectedPledges.Count(),
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgesQuery>(x => x.AccountId == null), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _pledgesController.GetPledges();
        var okObjectResult = actionResult as OkObjectResult;
        var response = okObjectResult.Value as GetPledgesResponse;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(response, Is.Not.Null);
        Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Assert.That(response.Pledges.Count(), Is.EqualTo(expectedPledges.Count()));
        Assert.That(response.TotalPledges, Is.EqualTo(expectedPledges.Count()));
    }

    [Test]
    public async Task GET_Account_Pledges_Returned()
    {
        // Arrange
        var accountId = _fixture.Create<long>();
        var expectedPledges = _fixture.CreateMany<GetPledgesResult.Pledge>();
        var sectors = _fixture.Create<IEnumerable<Sector>>();

        var result = new GetPledgesResult()
        {
            Items = expectedPledges.ToList(),
            TotalItems = expectedPledges.Count(),
        };

        _mockMediator
            .Setup(x => x.Send(It.Is<GetPledgesQuery>(x => x.AccountId == accountId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _pledgesController.GetPledges(sectors, accountId: accountId);
        var okObjectResult = actionResult as OkObjectResult;
        var response = okObjectResult.Value as GetPledgesResponse;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(response, Is.Not.Null);
        Assert.That(okObjectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Assert.That(response.Pledges.Count(), Is.EqualTo(expectedPledges.Count()));
        Assert.That(response.TotalPledges, Is.EqualTo(expectedPledges.Count()));
    }
}