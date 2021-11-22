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
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;

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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("accounts/{accountId}/applications/{applicationId}/decline-funding")]
        public async Task<IActionResult> DeclineFunding(int applicationId, long accountId, [FromBody] DeclineFundingRequest request)
        {
            var result = await _mediator.Send(new DeclineFundingCommand
            {
                ApplicationId = applicationId,
                AccountId = accountId,
                UserDisplayName = request.UserDisplayName,
                UserId = request.UserId,
            });

            if (result.Updated)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("accounts/{accountId}/applications/{applicationId}/withdraw")]
        public async Task<IActionResult> WithdrawApplication(int applicationId, long accountId, [FromBody] WithdrawApplicationRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new WithdrawApplicationCommand
            {
                ApplicationId = applicationId,
                AccountId = accountId,
                UserDisplayName = request.UserDisplayName,
                UserId = request.UserId
            }, cancellationToken);

            return Ok();
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
                StandardTitle = request.StandardTitle,
                StandardLevel = request.StandardLevel,
                StandardDuration = request.StandardDuration,
                StandardMaxFunding = request.StandardMaxFunding,
                StandardRoute = request.StandardRoute,
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

            return Ok((GetApplicationsResponse)query);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("applications/{applicationId}/debit")]
        public async Task<IActionResult> DebitApplication(int applicationId, [FromBody] DebitApplicationRequest request)
        {
            await _mediator.Send(new DebitApplicationCommand
            {
                ApplicationId = applicationId,
                NumberOfApprentices = request.NumberOfApprentices,
                Amount = request.Amount,
                MaxAmount = request.MaxAmount
            });

            return Ok();
        }
    }
}
