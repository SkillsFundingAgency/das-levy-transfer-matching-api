using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;

public class GetPledgesQueryHandler(LevyTransferMatchingDbContext dbContext) : IRequestHandler<GetPledgesQuery, GetPledgesResult>
{
    public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
    {
        var pledgesQuery = dbContext.Pledges.AsQueryable();

        if (request.AccountId.HasValue)
        {
            pledgesQuery = pledgesQuery.Where(x => x.EmployerAccount.Id == request.AccountId.Value);
        }

        if (request.Sectors != null && request.Sectors.Any())
        {
            var sectors = (Sector)request.Sectors.Cast<int>().Sum();
            pledgesQuery = pledgesQuery.Where(x => (x.Sectors & sectors) != 0 || x.Sectors == 0);
        }

        if (request.PledgeStatusFilter.HasValue)
        {
            pledgesQuery = pledgesQuery.Where(x => x.Status == request.PledgeStatusFilter);
        }

        var queryResult = await pledgesQuery
            .OrderByDescending(x => x.RemainingAmount)
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(x => new GetPledgesResult.Pledge
            {
                Amount = x.Amount,
                RemainingAmount = x.RemainingAmount,
                CreatedOn = x.CreatedOn,
                AccountId = x.EmployerAccount.Id,
                Id = x.Id,
                IsNamePublic = x.IsNamePublic,
                DasAccountName = x.EmployerAccount.Name,
                JobRoles = x.JobRoles.ToList(),
                Levels = x.Levels.ToList(),
                Sectors = x.Sectors.ToList(),
                Status = x.Status,
                Locations = x.Locations.Select(y => new LocationInformation { Name = y.Name, Geopoint = new[] { y.Latitude, y.Longitude } }).ToList(),
                ApplicationCount = Convert.ToInt32(x.Applications.Count)
            })
            .AsNoTracking()
            .AsSingleQuery()
            .ToListAsync(cancellationToken);

        var count = await pledgesQuery.CountAsync(cancellationToken: cancellationToken);

        return new GetPledgesResult
        {
            Items = queryResult.ToList(),
            TotalItems = count,
            PageSize = request.PageSize ?? int.MaxValue,
            Page = request.Page
        };
    }
}