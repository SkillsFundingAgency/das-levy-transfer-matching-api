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
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        Assert.That($"/accounts/{_apiRequest.AccountId}", Is.EqualTo(createdResult.Location));
    }

    [Test]
    public async Task POST_Create_Returns_OkResult()
    {
        _commandResult.Created = false;

        var actionResult = await _controller.CreateAccount(_apiRequest);

        var okResult = actionResult as OkResult;
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
    }


    [Test]
    public async Task GET_Returns_Result()
    {
        var actionResult = await _controller.GetAccount(_getAccountQueryResult.AccountId);

        var result = actionResult as ObjectResult;

        Assert.That(actionResult, Is.Not.Null);
        Assert.That(result, Is.Not.Null);
        var resultValue = result.Value as GetAccountResponse;
        Assert.That(resultValue.AccountId, Is.EqualTo(_getAccountQueryResult.AccountId));
        Assert.That(resultValue.AccountName, Is.EqualTo(_getAccountQueryResult.AccountName));
    }
}