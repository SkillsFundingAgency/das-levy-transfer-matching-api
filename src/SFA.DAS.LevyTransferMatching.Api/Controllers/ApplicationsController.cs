using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Api.Models.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("pledges/{pledgeId}/applications/{applicationId}")]
        public async Task<IActionResult> GetApplication(int pledgeId, int applicationId)
        {
            var queryResult = await _mediator.Send(new GetApplicationQuery()
            {
                PledgeId = pledgeId,
                ApplicationId = applicationId,
            });

            if (queryResult != null)
            {
                return Ok((GetApplicationResponse)queryResult);
            }

            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("applications/{applicationId}")]
        public async Task<IActionResult> GetApplication(int applicationId)
        {
            var queryResult = await _mediator.Send(new GetApplicationQuery()
            {
                ApplicationId = applicationId,
            });

            if (queryResult != null)
            {
                return Ok((GetApplicationResponse)queryResult);
            }

            return NotFound();
        }
		
		[HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [Route("pledges/{pledgeId}/applications/{applicationId}/approve")]
        public async Task<IActionResult> ApproveApplication(int pledgeId, int applicationId, [FromBody] ApproveApplicationRequest request)
        {
            await _mediator.Send(new ApproveApplicationCommand
            {
                PledgeId = pledgeId,
                ApplicationId = applicationId,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("pledges/{pledgeId}/applications/{applicationId}/reject")]
        public async Task<IActionResult> RejectApplication(int pledgeId, int applicationId, [FromBody] RejectApplicationRequest request)
        {
            await _mediator.Send(new RejectApplicationCommand
            {
                PledgeId = pledgeId,
                ApplicationId = applicationId,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("accounts/{accountId}/applications/{applicationId}/accept-funding")]
        public async Task<IActionResult> AcceptFunding(int applicationId, long accountId, [FromBody] AcceptFundingRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new AcceptFundingCommand
            {
                ApplicationId = applicationId,
                AccountId = accountId,
                UserDisplayName = request.UserDisplayName,
                UserId = request.UserId
            }, cancellationToken);

            if (result.Updated)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("pledges/{pledgeId}/applications/{applicationId}/undo-approval")]
        public async Task<IActionResult> UndoApplicationApproval(int pledgeId, int applicationId)
        {
            await _mediator.Send(new UndoApplicationApprovalCommand
            {
                PledgeId = pledgeId,
                ApplicationId = applicationId
            });

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("pledges/{pledgeId}/applications")]
        public async Task<IActionResult> CreateApplication(int pledgeId, [FromBody] CreateApplicationRequest request)
        {
            var commandResult = await _mediator.Send(new CreateApplicationCommand
            {
                PledgeId = pledgeId,
                EmployerAccountId = request.EmployerAccountId,
                Details = request.Details,
                StandardId = request.StandardId,
                NumberOfApprentices = request.NumberOfApprentices,
                StartDate = request.StartDate,
                HasTrainingProvider = request.HasTrainingProvider,
                Amount = request.Amount,
                Sectors = request.Sectors,
                Locations = request.Locations,
                AdditionalLocation = request.AdditionalLocation,
                SpecificLocation = request.SpecificLocation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddresses = request.EmailAddresses,
                BusinessWebsite = request.BusinessWebsite,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            var result = new CreatedResult(
                $"/pledges/{pledgeId}/applications/{commandResult.ApplicationId}",
                (CreateApplicationResponse)commandResult);

            return result;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("applications")]
        public async Task<IActionResult> GetApplications(int? pledgeId, long? accountId)
        {
            var query = await _mediator.Send(new GetApplicationsQuery
            {
                PledgeId = pledgeId,
                AccountId = accountId
            });

            return Ok(query);
        }
    }
}
