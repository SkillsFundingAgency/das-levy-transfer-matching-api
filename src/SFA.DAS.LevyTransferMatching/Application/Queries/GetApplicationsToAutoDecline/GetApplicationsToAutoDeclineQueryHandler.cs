using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

public class GetApplicationsToAutoDeclineQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext, LevyTransferMatchingApi configuration)
    : IRequestHandler<GetApplicationsToAutoDeclineQuery, GetApplicationsToAutoDeclineResult>
{
    public async Task<GetApplicationsToAutoDeclineResult> Handle(GetApplicationsToAutoDeclineQuery request, CancellationToken cancellationToken)
    {
        var implementationDate = configuration.AutoDeclineImplementationDate;

        var sixWeeksAgo = DateTime.Now.AddDays(-42);

        var result = await levyTransferMatchingDbContext.Applications
            .Include(x => x.StatusHistory)
            .Where(x =>
                x.Status == ApplicationStatus.Approved
                && x.CreatedOn > implementationDate
                && x.StatusHistory.Any(hist => hist.Status == ApplicationStatus.Approved
                && hist.CreatedOn < sixWeeksAgo))
            .AsNoTracking()
            .Select(x => x.Id)
            .Distinct()
            .ToListAsync(cancellationToken);

        return await Task.FromResult(new GetApplicationsToAutoDeclineResult
        {
            ApplicationIdsToDecline = result
        });
    }
}