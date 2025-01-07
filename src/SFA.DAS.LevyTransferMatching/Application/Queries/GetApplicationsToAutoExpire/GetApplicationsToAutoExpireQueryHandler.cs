using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoExpire;

public class GetApplicationsToAutoExpireQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext, LevyTransferMatchingApi configuration)
    : IRequestHandler<GetApplicationsToAutoExpireQuery, GetApplicationsToAutoExpireResult>
{
    public async Task<GetApplicationsToAutoExpireResult> Handle(GetApplicationsToAutoExpireQuery request, CancellationToken cancellationToken)
    {
        var implementationDate = configuration.AutoExpireApplicationsImplementationDate;

        var threeMonthsAgo = DateTime.Now.AddMonths(-3);

        var result = await levyTransferMatchingDbContext.Applications
            .Include(x => x.StatusHistory)
            .Where(x => 
                x.Status == ApplicationStatus.Accepted                
                && x.CreatedOn > implementationDate
                && x.StatusHistory.Any(hist => hist.Status == ApplicationStatus.Accepted
                && hist.CreatedOn < threeMonthsAgo))
            .AsNoTracking()
            .Select(x => x.Id)
            .Distinct()
            .ToListAsync(cancellationToken);
      
        return new GetApplicationsToAutoExpireResult
        {
            ApplicationIdsToExpire = result
        };
    }
}
