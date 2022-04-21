using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;
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
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetPledges([FromQuery] IEnumerable<Sector> sectors = null, long? accountId = null, int page = 0, int? pageSize = null, PledgeStatus? pledgeStatusFilter = null)
        {
            var result = await _mediator.Send(new GetPledgesQuery
            {
                Sectors = sectors,
                AccountId = accountId,
                Page = page,
                PageSize = pageSize,
                PledgeStatusFilter = pledgeStatusFilter
            });

            var response = new GetPledgesResponse
            {
                Pledges = result.Items.Select(x => (GetPledgesResponse.Pledge)x),
                TotalPledges = result.TotalItems,
                TotalPages = result.TotalPages,
                PageSize = result.PageSize,
                Page = result.Page
            };

            return Ok(response);
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
        [Route("pledges/{pledgeId}/debit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DebitPledge(int pledgeId, [FromBody] DebitPledgeRequest request)
        {
            var result = await _mediator.Send(new DebitPledgeCommand
            {
                PledgeId = pledgeId,
                ApplicationId = request.ApplicationId,
                Amount = request.Amount
            });

            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("accounts/{accountId}/pledges")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePledge(long accountId, [FromBody] CreatePledgeRequest request)
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

        [HttpPost]
        [Route("pledges/{pledgeId}/close")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ClosePledge(int pledgeId, [FromBody] ClosePledgeRequest request)
        {
            try
            {
                await _mediator.Send(new ClosePledgeCommand
                {
                    PledgeId = pledgeId,
                    UserId = request.UserId,
                    UserDisplayName = request.UserDisplayName
                });

                return Ok();

            }
            catch (AggregateNotFoundException<Pledge>)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("pledges/{pledgeId}/credit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreditPledge(int pledgeId, [FromBody] CreditPledgeRequest request)
        {
            await _mediator.Send(new CreditPledgeCommand
            {
                PledgeId = pledgeId,
                ApplicationId = request.ApplicationId,
                Amount = request.Amount
            });

            return Ok();
        }
    }
}