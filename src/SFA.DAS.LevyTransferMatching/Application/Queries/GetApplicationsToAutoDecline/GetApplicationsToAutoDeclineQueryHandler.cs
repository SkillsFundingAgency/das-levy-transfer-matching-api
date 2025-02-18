using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

public class GetApplicationsToAutoDeclineQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext, LevyTransferMatchingApi configuration,
    ILogger<GetApplicationsToAutoDeclineQueryHandler> logger)
    : IRequestHandler<GetApplicationsToAutoDeclineQuery, GetApplicationsToAutoDeclineResult>
{
    public async Task<GetApplicationsToAutoDeclineResult> Handle(GetApplicationsToAutoDeclineQuery request, CancellationToken cancellationToken)
    {
        var implementationDate = configuration.AutoDeclineImplementationDate;

        var sixWeeksAgo = DateTime.UtcNow.AddDays(-42);
        var sixWeeksAgoUTC = DateTime.UtcNow.AddDays(-42);

        logger.LogInformation("GetApplicationsToAutoDecline - sixWeeksAgo : {date}", sixWeeksAgo);
        logger.LogInformation("GetApplicationsToAutoDecline - implementationDate : {date}", implementationDate);
        logger.LogInformation("GetApplicationsToAutoDecline - UTCnow : {date}", DateTime.UtcNow);

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