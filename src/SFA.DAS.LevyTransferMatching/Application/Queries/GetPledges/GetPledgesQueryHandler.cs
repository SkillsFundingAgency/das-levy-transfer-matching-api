using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using static SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges.GetPledgesResult;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public GetPledgesQueryHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var pledgeEntriesQuery = _dbContext.Pledges.AsQueryable();

            if (request.AccountId.HasValue)
            {
                pledgeEntriesQuery = pledgeEntriesQuery.Where(x => x.EmployerAccount.Id == request.AccountId.Value);
            }

            var pledgeEntries = await pledgeEntriesQuery
                                        .Include(x => x.EmployerAccount)
                                        .Include(x => x.Applications)
                                        .ToListAsync();

            var pledges = pledgeEntries
                .Select(
                    x => new Pledge()
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
                        ApplicationCount = x.Applications.Count
                    })
                .OrderByDescending(x => x.Amount);

            return new GetPledgesResult()
            {
                Items = pledges,
                TotalItems = pledges.Count(),
            };
        }
    }
}