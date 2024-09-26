using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Api.Models.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateCostProjection;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
public class ApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [Route("pledges/{pledgeId:int}/applications/{applicationId:int}")]
    public async Task<IActionResult> GetApplication(int pledgeId, int applicationId)
    {
        var queryResult = await mediator.Send(new GetApplicationQuery()
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
    [Route("applications/{applicationId:int}")]
    public async Task<IActionResult> GetApplication(int applicationId)
    {
        var queryResult = await mediator.Send(new GetApplicationQuery()
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Route("pledges/{pledgeId:int}/applications/{applicationId:int}/approve")]
    public async Task<IActionResult> ApproveApplication(int pledgeId, int applicationId, [FromBody] ApproveApplicationRequest request)
    {
        await mediator.Send(new ApproveApplicationCommand
        {
            PledgeId = pledgeId,
            ApplicationId = applicationId,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
            AutomaticApproval = request.AutomaticApproval
        });

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Route("pledges/{pledgeId:int}/applications/{applicationId:int}/reject")]
    public async Task<IActionResult> RejectApplication(int pledgeId, int applicationId, [FromBody] RejectApplicationRequest request)
    {
        await mediator.Send(new RejectApplicationCommand
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
    [Route("accounts/{accountId:long}/applications/{applicationId:int}/accept-funding")]
    public async Task<IActionResult> AcceptFunding(int applicationId, long accountId, [FromBody] AcceptFundingRequest request, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new AcceptFundingCommand
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
    [Route("accounts/{accountId:long}/applications/{applicationId:int}/decline-funding")]
    public async Task<IActionResult> DeclineFunding(int applicationId, long accountId, [FromBody] DeclineFundingRequest request)
    {
        var result = await mediator.Send(new DeclineFundingCommand
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
    [Route("accounts/{accountId:long}/applications/{applicationId:int}/withdraw")]
    public async Task<IActionResult> WithdrawApplication(int applicationId, long accountId, [FromBody] WithdrawApplicationRequest request, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new WithdrawApplicationCommand
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
    [Route("pledges/{pledgeId:int}/applications/{applicationId:int}/undo-approval")]
    public async Task<IActionResult> UndoApplicationApproval(int pledgeId, int applicationId)
    {
        await mediator.Send(new UndoApplicationApprovalCommand
        {
            PledgeId = pledgeId,
            ApplicationId = applicationId
        });

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("pledges/{pledgeId:int}/applications")]
    public async Task<IActionResult> CreateApplication(int pledgeId, [FromBody] CreateApplicationRequest request)
    {
        var commandResult = await mediator.Send(new CreateApplicationCommand
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
    public async Task<IActionResult> GetApplications([FromQuery] GetApplicationsRequest request)
    {
        var query = await mediator.Send(new GetApplicationsQuery
        {
            PledgeId = request.PledgeId,
            AccountId = request.AccountId,
            SenderAccountId = request.SenderAccountId,
            ApplicationStatusFilter = request.ApplicationStatusFilter,
            Page = request.Page,
            PageSize = request.PageSize,
            SortOrder = request.SortOrder,
            SortDirection = request.SortDirection
        });

        return Ok((GetApplicationsResponse)query);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("applications/{applicationId:int}/debit")]
    public async Task<IActionResult> DebitApplication(int applicationId, [FromBody] DebitApplicationRequest request)
    {
        await mediator.Send(new DebitApplicationCommand
        {
            ApplicationId = applicationId,
            NumberOfApprentices = request.NumberOfApprentices,
            Amount = request.Amount,
            MaxAmount = request.MaxAmount
        });

        return Ok();
    }


    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("applications/{applicationId:int}/recalculate-cost-projection")]
    public async Task<IActionResult> RecalculateCostProjection(int applicationId)
    {
        await mediator.Send(new RecalculateCostProjectionCommand
        {
            ApplicationId = applicationId,
        });

        return Ok();
    }
}