using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsForAutomaticApproval;

public class GetApplicationsForAutomaticApprovalQueryHandler(LevyTransferMatchingDbContext dbContext)
    : IRequestHandler<GetApplicationsForAutomaticApprovalQuery, GetApplicationsForAutomaticApprovalResult>
{
    public async Task<GetApplicationsForAutomaticApprovalResult> Handle(GetApplicationsForAutomaticApprovalQuery request, CancellationToken cancellationToken)
    {
        var applications = dbContext.Applications
            .AsNoTracking()
            .Where(application => application.Status == ApplicationStatus.Pending);

        if (request.PledgeId.HasValue)
        {
            applications = applications.Where(application => application.PledgeId == request.PledgeId);
        }

        var rows = await applications
            .OrderBy(application => application.PledgeId)
            .ThenBy(application => application.CreatedOn)
            .Select(application => new
            {
                application.Id,
                EmployerAccountId = application.EmployerAccount.Id,
                application.PledgeId,
                application.MatchPercentage,
                PledgeAutomaticApprovalOption = application.Pledge.AutomaticApprovalOption,
                Amount = application.GetCost(),
                PledgeRemainingAmount = application.Pledge.RemainingAmount,
                application.CreatedOn,
                application.Status
            })
            .ToListAsync(cancellationToken);

        return new GetApplicationsForAutomaticApprovalResult
        {
            Applications = rows.Select(application => new GetApplicationsForAutomaticApprovalResult.Application
            {
                Id = application.Id,
                EmployerAccountId = application.EmployerAccountId,
                PledgeId = application.PledgeId,
                MatchPercentage = application.MatchPercentage,
                PledgeAutomaticApprovalOption = application.PledgeAutomaticApprovalOption,
                Amount = application.Amount,
                PledgeRemainingAmount = application.PledgeRemainingAmount,
                CreatedOn = application.CreatedOn,
                Status = application.Status.ToString()
            }).ToList()
        };
    }
}
