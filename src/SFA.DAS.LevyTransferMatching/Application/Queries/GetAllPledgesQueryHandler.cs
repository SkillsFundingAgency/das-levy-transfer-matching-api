using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            return new GetAllPledgesResult
            {
                Pledges = pledges
            };
        }
    }
}
