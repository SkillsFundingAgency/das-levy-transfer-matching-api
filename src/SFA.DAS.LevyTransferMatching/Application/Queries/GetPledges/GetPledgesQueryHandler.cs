using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var pledges = await _dbContext.Pledges
                .Where(x => (request.Id.HasValue && x.Id == request.Id.Value) || !request.Id.HasValue)
                .Include(x => x.EmployerAccount)
                .ToListAsync();

            return new GetPledgesResult(
                pledges.Select(
                    x => new Pledge()
                    {
                        Amount = x.Amount,
                        CreatedOn = x.CreatedOn,
                        AccountId = x.EmployerAccount.Id,
                        Id = x.Id,
                        IsNamePublic = x.IsNamePublic,
                        DasAccountName = x.EmployerAccount.Name,
                        JobRoles = x.JobRoles.GetFlags<JobRole>(),
                        Levels = x.Levels.GetFlags<Level>(),
                        Sectors = x.Sectors.GetFlags<Sector>(),
                    }));
        }
    }
}
