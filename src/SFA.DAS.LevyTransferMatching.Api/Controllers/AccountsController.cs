using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.CreateAccount;
using SFA.DAS.LevyTransferMatching.Api.Models.GetAccount;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("{accountId}")]
        public async Task<IActionResult> GetAccount(long accountId)
        {
            var queryResult = await _mediator.Send(new GetAccountQuery
            {
                AccountId = accountId
            });

            if(queryResult == null)
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
        [ProducesResponseType((int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            var commandResult = await _mediator.Send(new CreateAccountCommand
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
}
