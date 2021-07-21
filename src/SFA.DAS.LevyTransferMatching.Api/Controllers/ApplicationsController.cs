using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("accounts/{accountId}/pledges/{pledgeId}/applications")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApplication(long accountId, int pledgeId, [FromBody] CreateApplicationRequest request)
        {
            var commandResult = await _mediator.Send(new CreateApplicationCommand
            {
                EmployerAccountId = accountId,
                PledgeId = pledgeId,
                ReceiverEmployerAccountId = request.ReceiverEmployerAccountId
            });

            var result = new CreatedResult(
                $"/accounts/{accountId}/pledges/{pledgeId}/applications/{commandResult.ApplicationId}",
                (CreateApplicationResponse)commandResult);

            return result;
        }
    }
}
