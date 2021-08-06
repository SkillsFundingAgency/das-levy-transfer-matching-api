using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Api.Models.CreatePledge;
using SFA.DAS.LevyTransferMatching.Api.Models.GetPledges;
using SFA.DAS.LevyTransferMatching.Api.Models.GetPledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class PledgesController : Controller
    {
        private readonly IMediator _mediator;

        public PledgesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("pledges")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return Ok(new GetPledgesResponse(result.Select(x => (GetPledgesResponse.Pledge)x)));
        }

        [HttpGet]
        [Route("pledges/{pledgeId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPledge(int pledgeId)
        {
            var result = await _mediator.Send(new GetPledgeQuery()
            {
                Id = pledgeId,
            });

            if (result != null)
            {
                return Ok((GetPledgeResponse)result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("accounts/{accountId}/pledges")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePledge(long accountId, [FromBody]CreatePledgeRequest request)
        {
            var commandResult = await _mediator.Send(new CreatePledgeCommand
            {
                AccountId = accountId,
                Amount = request.Amount,
                IsNamePublic = request.IsNamePublic,
                DasAccountName = request.DasAccountName,
                JobRoles = request.JobRoles,
                Levels = request.Levels,
                Sectors = request.Sectors,
                Locations = request.Locations,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            var result = new CreatedResult(
                $"/accounts/{accountId}/pledges/{commandResult.Id}",
                (CreatePledgeResponse)commandResult);

            return result;
        }
    }
}