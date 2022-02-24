using System.Collections;
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
            if (request.ApplicationStatusFilter.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(x => x.Status == request.ApplicationStatusFilter);
            }

            if (request.PledgeId.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(x => x.Pledge.Id == request.PledgeId);
            }

            if (request.AccountId.HasValue)
            {
                applicationsQuery = applicationsQuery.Where(x => x.EmployerAccount.Id == request.AccountId);
            }

            var queryResult = await applicationsQuery
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.EmployerAccount.Name)
                .Skip(request.Offset)
                .Take(request.Limit)
                .Select(x => new GetApplicationsResult.Application
                {
                    PledgeId = x.Pledge.Id,
                    EmployerAccountId = x.EmployerAccount.Id,
                    DasAccountName = request.PledgeId.HasValue ? x.EmployerAccount.Name : x.Pledge.EmployerAccount.Name,
                    Id = x.Id,
                    Sectors = x.Sectors.ToList(),
                    BusinessWebsite = x.BusinessWebsite,
                    Details = x.Details,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HasTrainingProvider = x.HasTrainingProvider,
                    NumberOfApprentices = x.NumberOfApprentices,
                    NumberOfApprenticesUsed = x.NumberOfApprenticesUsed,
                    StandardId = x.StandardId,
                    StandardTitle = x.StandardTitle,
                    StandardLevel = x.StandardLevel,
                    StandardDuration = x.StandardDuration,
                    StandardMaxFunding = x.StandardMaxFunding,
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
                    Locations = x.ApplicationLocations.Select(location =>
                        new GetApplicationsResult.Application.ApplicationLocation
                            { Id = location.Id, PledgeLocationId = location.PledgeLocationId }).ToList(),
                    AdditionalLocations = x.AdditionalLocation,
                    SpecificLocation = x.SpecificLocation,
                    SenderEmployerAccountId = x.Pledge.EmployerAccount.Id,
                    SenderEmployerAccountName = x.Pledge.EmployerAccount.Name,
                    CostProjections = x.ApplicationCostProjections.Select(p =>
                        new GetApplicationsResult.Application.CostProjection
                            { FinancialYear = p.FinancialYear, Amount = p.Amount }).ToList(),
                    MatchPercentage = x.MatchPercentage,
                    MatchSector = x.MatchSector,
                    MatchJobRole = x.MatchJobRole,
                    MatchLevel = x.MatchLevel,
                    MatchLocation = x.MatchLocation
                })
                .AsNoTracking()
                .AsSingleQuery()
                .ToListAsync(cancellationToken);

            var count = await applicationsQuery.CountAsync(cancellationToken: cancellationToken);

            return new GetApplicationsResult
            {
                Items = queryResult,
                Page = request.Page,
                TotalItems = count,
                PageSize = request.PageSize ?? int.MaxValue
            };
        }
    }
}