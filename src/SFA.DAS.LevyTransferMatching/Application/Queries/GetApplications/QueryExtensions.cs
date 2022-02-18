using System;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public static class QueryExtensions
    {
        public static IQueryable<Data.Models.Application> Filter(this IQueryable<Data.Models.Application> queryable, GetApplicationsQuery request)
        {
            var result = queryable;

            if (request.ApplicationStatusFilter.HasValue)
            {
                result = result.Where(x => x.Status == request.ApplicationStatusFilter);
            }

            if (request.PledgeId.HasValue)
            {
                result = result.Where(x => x.Pledge.Id == request.PledgeId);
            }

            if (request.AccountId.HasValue)
            {
                result = result.Where(x => x.EmployerAccount.Id == request.AccountId);
            }

            return result;
        }

        public static IQueryable<Data.Models.Application> Sort(this IQueryable<Data.Models.Application> queryable,
            GetApplicationsSortOrder sortOrder, SortDirection sortDirection, DateTime now)
        {
            var result = sortOrder switch
            {
                GetApplicationsSortOrder.CriteriaMatch => sortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(x => x.MatchPercentage)
                    : queryable.OrderByDescending(x => x.MatchPercentage),
                GetApplicationsSortOrder.ApplicationDate => queryable.OrderByDescending(x => x.CreatedOn),
                GetApplicationsSortOrder.Applicant => sortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(x => x.EmployerAccount.Name)
                    : queryable.OrderByDescending(x => x.EmployerAccount.Name),
                GetApplicationsSortOrder.Duration => sortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(x => x.StandardDuration)
                    : queryable.OrderByDescending(x => x.StandardDuration),
                GetApplicationsSortOrder.CurrentFinancialYearAmount => sortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(x => x.ApplicationCostProjections
                        .Where(p => p.FinancialYear == now.GetFinancialYear())
                        .Sum(p=> p.Amount))
                    : queryable.OrderByDescending(x => x.ApplicationCostProjections
                        .Where(p => p.FinancialYear == now.GetFinancialYear())
                        .Sum(p => p.Amount)),
                GetApplicationsSortOrder.Status => sortDirection == SortDirection.Ascending
                    ? queryable.OrderBy(x => x.Status.ToString())
                    : queryable.OrderByDescending(x => x.Status.ToString()),
                _ => null
            };

            if (result == null)
            {
                throw new InvalidOperationException("Invalid SortOrder");
            }

            return sortOrder != GetApplicationsSortOrder.Applicant ? result.ThenBy(x => x.EmployerAccount.Name) : result;
        }
    }
}
