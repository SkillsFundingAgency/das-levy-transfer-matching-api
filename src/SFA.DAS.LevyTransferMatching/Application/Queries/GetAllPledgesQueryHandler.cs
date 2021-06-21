using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries
{
    public class GetAllPledgesQueryHandler : IRequestHandler<GetAllPledgesQuery, GetAllPledgesResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public GetAllPledgesQueryHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAllPledgesResult> Handle(GetAllPledgesQuery request, CancellationToken cancellationToken)
        {
            var pledges = await _dbContext.Pledges.ToListAsync();
            var accounts = await _dbContext.EmployerAccounts.ToListAsync();

            return new GetAllPledgesResult(
                pledges.Select(
                    x => new Pledge()
                    {
                        Amount = x.Amount,
                        CreatedOn = x.CreatedOn,
                        AccountId = x.EmployerAccountId,
                        Id = x.Id,
                        IsNamePublic = x.IsNamePublic,
                        DasAccountName = accounts.FirstOrDefault(y => y.Id == x.EmployerAccountId).Name,
                        JobRoles = x.JobRoles.GetFlags<JobRole>(),
                        Levels = x.Levels.GetFlags<Level>(),
                        Sectors = x.Sectors.GetFlags<Sector>(),
                    }).OrderByDescending(x => x.Amount));
        }
    }
}
