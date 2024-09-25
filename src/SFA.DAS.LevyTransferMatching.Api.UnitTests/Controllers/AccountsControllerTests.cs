using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.CreateAccount;
using SFA.DAS.LevyTransferMatching.Api.Models.GetAccount;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers;

[TestFixture]
public class AccountsControllerTests
{
    private Fixture _fixture;
    private Mock<IMediator> _mediator;
    private AccountsController _controller;
    private CreateAccountCommandResult _commandResult;
    private CreateAccountRequest _apiRequest;
    private GetAccountQueryResult _getAccountQueryResult;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _mediator = new Mock<IMediator>();
        _controller = new AccountsController(_mediator.Object);

        _commandResult = new CreateAccountCommandResult { Created = true };
        _apiRequest = _fixture.Create<CreateAccountRequest>();

        _mediator
            .Setup(x => x.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_commandResult);

        _mediator
            .Setup(x => x.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_commandResult);

        _getAccountQueryResult = _fixture.Create<GetAccountQueryResult>();
        _mediator
            .Setup(x => x.Send(It.IsAny<GetAccountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getAccountQueryResult);
    }

    [Test]
    public async Task POST_Create_Returns_CreatedResult()
    {
        _commandResult.Created = true;

        var actionResult = await _controller.CreateAccount(_apiRequest);

        var createdResult = actionResult as CreatedResult;
        actionResult.Should().NotBeNull();
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
        $"/accounts/{_apiRequest.AccountId}".Should().Be(createdResult.Location);
    }

    [Test]
    public async Task POST_Create_Returns_OkResult()
    {
        _commandResult.Created = false;

        var actionResult = await _controller.CreateAccount(_apiRequest);

        var okResult = actionResult as OkResult;
        actionResult.Should().NotBeNull();
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }


    [Test]
    public async Task GET_Returns_Result()
    {
        var actionResult = await _controller.GetAccount(_getAccountQueryResult.AccountId);

        var result = actionResult as ObjectResult;

        actionResult.Should().NotBeNull();
        result.Should().NotBeNull();
        var resultValue = result.Value as GetAccountResponse;
        resultValue.AccountId.Should().Be(_getAccountQueryResult.AccountId);
        resultValue.AccountName.Should().Be(_getAccountQueryResult.AccountName);
    }
}