using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoExpire;

public class GetApplicationsToAutoExpireQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext)
    : IRequestHandler<GetApplicationsToAutoExpireQuery, GetApplicationsToAutoExpireResult>
{
    public async Task<GetApplicationsToAutoExpireResult> Handle(GetApplicationsToAutoExpireQuery request, CancellationToken cancellationToken)
    {
        var applicationsToExpireQuery = levyTransferMatchingDbContext.Applications
             .FromSql($"EXEC [dbo].[GetApplicationsToAutoExpire]");

        var applications = await applicationsToExpireQuery.ToListAsync(cancellationToken);

        return await Task.FromResult(new GetApplicationsToAutoExpireResult
        {
            ApplicationsToExpire = applications.Select(app => app.Id)
        });
    }
}
