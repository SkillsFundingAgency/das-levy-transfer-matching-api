using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public GetApplicationsQueryHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            if (request.PledgeId > 0)
            {
                return new GetApplicationsResult(await _dbContext.Applications
                    .Where(x => x.Pledge.Id == request.PledgeId)
                    .OrderByDescending(x => x.CreatedOn)
                    .ThenBy(x => x.EmployerAccount.Name)
                    .Select(x => new Models.Application
                    {
                        Amount = x.Amount,
                        PledgeId = x.Pledge.Id,
                        DasAccountName = x.EmployerAccount.Name,
                        Id = x.Id,
                        Sectors = x.Sectors.ToList(),
                        BusinessWebsite = x.BusinessWebsite,
                        Details = x.Details,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        HasTrainingProvider = x.HasTrainingProvider,
                        NumberOfApprentices = x.NumberOfApprentices,
                        Postcode = x.Postcode,
                        StandardId = x.StandardId,
                        StartDate = x.StartDate,
                        EmailAddresses = x.EmailAddresses.Any()
                            ? x.EmailAddresses.Select(email => email.EmailAddress)
                            : null,
                        CreatedOn = x.CreatedOn,
                        Status = x.Status
                    })
                    .ToListAsync(cancellationToken));
            }

            return new GetApplicationsResult(
                await _dbContext.Applications
                .Where(o => o.Pledge.EmployerAccount.Id == request.AccountId)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new Models.Application
                {
                    PledgeId = x.PledgeId,
                    Amount = x.Amount,
                    DasAccountName = x.EmployerAccount.Name,
                    Id = x.Id,
                    Sectors = x.Sectors.ToList(),
                    BusinessWebsite = x.BusinessWebsite,
                    Details = x.Details,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HasTrainingProvider = x.HasTrainingProvider,
                    NumberOfApprentices = x.NumberOfApprentices,
                    Postcode = x.Postcode,
                    StandardId = x.StandardId,
                    StartDate = x.StartDate,
                    EmailAddresses = x.EmailAddresses.Any()
                        ? x.EmailAddresses.Select(email => email.EmailAddress)
                        : null,
                    CreatedOn = x.CreatedOn,
                    Status = x.Status
                }).ToListAsync(cancellationToken));

        }
    }
}