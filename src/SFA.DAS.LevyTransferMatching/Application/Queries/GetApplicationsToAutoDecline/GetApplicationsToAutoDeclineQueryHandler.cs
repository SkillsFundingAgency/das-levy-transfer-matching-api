using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

public class GetApplicationsToAutoDeclineQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext)
    : IRequestHandler<GetApplicationsToAutoDeclineQuery, GetApplicationsToAutoDeclineResult>
{
    public async Task<GetApplicationsToAutoDeclineResult> Handle(GetApplicationsToAutoDeclineQuery request, CancellationToken cancellationToken)
    {
        var applicationsToDeclineQuery = levyTransferMatchingDbContext.Applications
             .FromSql($"EXEC [dbo].[GetApplicationsToAutoDecline]");

        var applications = await applicationsToDeclineQuery.ToListAsync(cancellationToken);

        return await Task.FromResult(new GetApplicationsToAutoDeclineResult
        {
            ApplicationsToDecline = applications.Select(app => app.Id)
        });
    }
}