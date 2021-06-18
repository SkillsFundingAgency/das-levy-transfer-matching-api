using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using System.Threading.Tasks;
using FluentValidation;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class PledgesController : Controller
    {
        private readonly IMediator _mediator;

        public PledgesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("pledges")]
        public async Task<IActionResult> Pledges()
        {
            var result = await _mediator.Send(new GetPledgesQuery());

            return Ok(result);
        }

        [HttpPost]
        [Route("accounts/{accountId}/pledges")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create(long accountId, [FromBody]CreatePledgeRequest request)
        {
            IActionResult result = null;

            try
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
                });

                result = new CreatedResult(
                    $"/accounts/{accountId}/pledges/{commandResult.Id}",
                    new CreatePledgeResponse()
                    {
                        Id = commandResult.Id,
                    });
            }
            catch (ValidationException validationException)
            {
                // This can all get common-ised if it's needed elsewhere.
                // Either through base classes for controllers, or something
                // global in the pipeline that picks up on exceptions like
                // this.
                result = new BadRequestObjectResult(new FluentValidationErrorResponse()
                {
                    Errors = validationException.Errors,
                });
            }

            return result;
        }
    }
}