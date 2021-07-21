using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("pledges/{pledgeId}/applications")]
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
        public async Task<IActionResult> CreateApplication(int pledgeId, [FromBody] CreateApplicationRequest request)
        {
            var commandResult = await _mediator.Send(new CreateApplicationCommand
            {
                PledgeId = pledgeId,
                EmployerAccountId = request.EmployerAccountId
            });

            var result = new CreatedResult(
                $"/pledges/{pledgeId}/applications/{commandResult.ApplicationId}",
                (CreateApplicationResponse)commandResult);

            return result;
        }
    }
}
