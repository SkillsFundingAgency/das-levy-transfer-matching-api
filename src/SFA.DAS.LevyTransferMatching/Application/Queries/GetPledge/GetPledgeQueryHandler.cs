﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;

public class GetPledgeQueryHandler(LevyTransferMatchingDbContext dbContext) : IRequestHandler<GetPledgeQuery, GetPledgeResult>
{
    public async Task<GetPledgeResult> Handle(GetPledgeQuery request, CancellationToken cancellationToken)
    {
        var pledge = await dbContext.Pledges
            .Where(x => x.Id == request.Id)
            .Include(x => x.EmployerAccount)
            .Include(x => x.Locations)
            .SingleOrDefaultAsync(cancellationToken);

            if (pledge != null)
            {
                return new GetPledgeResult
                {
                    Amount = pledge.Amount,
                    CreatedOn = pledge.CreatedOn,
                    AccountId = pledge.EmployerAccount.Id,
                    Id = pledge.Id,
                    IsNamePublic = pledge.IsNamePublic,
                    DasAccountName = pledge.EmployerAccount.Name,
                    JobRoles = pledge.JobRoles.ToList(),
                    Levels = pledge.Levels.ToList(),
                    Sectors = pledge.Sectors.ToList(),
                    Status = pledge.Status,
                    Locations = pledge.Locations.Select(x => new GetPledgeResult.Location { Id = x.Id, Name = x.Name }),
                    RemainingAmount = pledge.RemainingAmount,
                    AutomaticApprovalOption = pledge.AutomaticApprovalOption
                };
            }

        return null;
    }
}