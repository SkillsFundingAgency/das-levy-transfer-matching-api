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
            IQueryable<Data.Models.Application> applicationsQuery = _dbContext.Applications;

            if (request.PledgeId.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(x => x.Pledge.Id == request.PledgeId);
            }

            if (request.AccountId.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(x => x.EmployerAccount.Id == request.AccountId);
            }

            return new GetApplicationsResult(await applicationsQuery
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.EmployerAccount.Name)
                .Select(x => new GetApplicationsResult.Application
                {
                    PledgeId = x.Pledge.Id,
                    DasAccountName = request.PledgeId.HasValue ? x.EmployerAccount.Name : x.Pledge.EmployerAccount.Name,
                    Id = x.Id,
                    Sectors = x.Sectors.ToList(),
                    BusinessWebsite = x.BusinessWebsite,
                    Details = x.Details,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HasTrainingProvider = x.HasTrainingProvider,
                    NumberOfApprentices = x.NumberOfApprentices,
                    StandardId = x.StandardId,
                    StandardTitle = x.StandardTitle,
                    StandardLevel = x.StandardLevel,
                    StandardDuration = x.StandardDuration,
                    StandardMaxFunding =  x.StandardMaxFunding,
                    StandardRoute = x.StandardRoute,
                    Amount = x.Amount,
                    TotalAmount = x.TotalAmount,
                    StartDate = x.StartDate,
                    EmailAddresses = x.EmailAddresses.Any()
                        ? x.EmailAddresses.Select(email => email.EmailAddress)
                        : null,
                    CreatedOn = x.CreatedOn,
                    Status = x.Status,
                    IsNamePublic = x.Pledge.IsNamePublic,
                    Locations = x.ApplicationLocations.Select(location => new GetApplicationsResult.Application.ApplicationLocation { Id = location.Id, PledgeLocationId = location.PledgeLocationId }).ToList()
                })
                .ToListAsync(cancellationToken));
        }
    }
}