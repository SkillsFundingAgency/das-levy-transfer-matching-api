using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;

        public GetApplicationsQueryHandler(LevyTransferMatchingDbContext dbContext, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _dateTimeService = dateTimeService;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeService.UtcNow;

            var applicationsQuery = _dbContext.Applications
                .Include(x => x.ApplicationCostProjections)
                .AsQueryable()
                .Filter(request)
                .Sort(request.SortOrder, request.SortDirection, now);

            var queryResult = await applicationsQuery
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
                    Amount = x.CostingModel == ApplicationCostingModel.Original
                        ? Convert.ToInt32(x.ApplicationCostProjections.Where(p => p.FinancialYear == now.GetFinancialYear()).Sum(p => p.Amount))
                        : x.Amount,
                    TotalAmount = x.TotalAmount,
                    CurrentFinancialYearAmount = Convert.ToInt32(x.ApplicationCostProjections.Where(p=> p.FinancialYear == now.GetFinancialYear()).Sum(p => p.Amount)),
                    StartDate = x.StartDate,
                    EmailAddresses = Convert.ToInt32(x.EmailAddresses.Count()) > 0
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
                    CostProjections = x.ApplicationCostProjections.OrderBy(p=> p.FinancialYear).Select(p =>
                        new GetApplicationsResult.Application.CostProjection
                            { FinancialYear = p.FinancialYear, Amount = p.Amount }).ToList(),
                    MatchPercentage = x.MatchPercentage,
                    MatchSector = x.MatchSector,
                    MatchJobRole = x.MatchJobRole,
                    MatchLevel = x.MatchLevel,
                    MatchLocation = x.MatchLocation,
                    CostingModel = x.CostingModel,
                    PledgeRemainingAmount = x.Pledge.RemainingAmount,
                    AutomaticApprovalOption = x.Pledge.AutomaticApprovalOption
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