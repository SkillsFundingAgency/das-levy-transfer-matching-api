using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.CreateAccount;
using SFA.DAS.LevyTransferMatching.Api.Models.GetAccount;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers;

[Route("accounts")]
[ApiVersion("1.0")]
[ApiController]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Route("{accountId:long}")]
    public async Task<IActionResult> GetAccount(long accountId)
    {
        var queryResult = await mediator.Send(new GetAccountQuery
        {
            AccountId = accountId
        });

        if (queryResult == null)
        {
            return new NotFoundResult();
        }

        return new ObjectResult(new GetAccountResponse
        {
            AccountId = queryResult.AccountId,
            AccountName = queryResult.AccountName
        });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var commandResult = await mediator.Send(new CreateAccountCommand
        {
            AccountId = request.AccountId,
            AccountName = request.AccountName
        });

        if (commandResult.Created)
        {
            return new CreatedResult($"/accounts/{request.AccountId}", null);
        }

        return new OkResult();
    }
}