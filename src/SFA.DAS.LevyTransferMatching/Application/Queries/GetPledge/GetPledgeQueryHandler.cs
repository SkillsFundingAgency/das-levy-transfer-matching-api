using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge
{
    public class GetPledgeQueryHandler : IRequestHandler<GetPledgeQuery, GetPledgeResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public GetPledgeQueryHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetPledgeResult> Handle(GetPledgeQuery request, CancellationToken cancellationToken)
        {
            var pledges = await _dbContext.Pledges
                .Where(x => x.Id == request.Id)
                .Include(x => x.EmployerAccount)
                .ToListAsync();

            var pledge = pledges.SingleOrDefault();

            if (pledge != null)
            {
                return new GetPledgeResult()
                {
                    Amount = pledge.Amount,
                    CreatedOn = pledge.CreatedOn,
                    AccountId = pledge.EmployerAccount.Id,
                    Id = pledge.Id,
                    IsNamePublic = pledge.IsNamePublic,
                    DasAccountName = pledge.EmployerAccount.Name,
                    JobRoles = pledge.JobRoles.GetFlags<JobRole>(),
                    Levels = pledge.Levels.GetFlags<Level>(),
                    Sectors = pledge.Sectors.GetFlags<Sector>(),
                };
            }
            else
            {
                return null;
            }
        }
    }
}