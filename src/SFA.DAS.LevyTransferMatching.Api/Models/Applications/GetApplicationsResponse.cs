using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationsResponse
    {
        public static implicit operator GetApplicationsResponse(GetApplicationsResult source)
        {
            var result = new GetApplicationsResponse
            {
                Page = source.Page,
                TotalItems = source.TotalItems,
                PageSize = source.PageSize,
                TotalPages = source.TotalPages,
                Applications = source.Items.Select(application => new Application
                {
                    Id = application.Id,
                    EmployerAccountId = application.EmployerAccountId,
                    DasAccountName = application.DasAccountName,
                    PledgeId = application.PledgeId,
                    Details = application.Details,
                    StandardId = application.StandardId,
                    StandardTitle = application.StandardTitle,
                    StandardLevel = application.StandardLevel,
                    StandardDuration = application.StandardDuration,
                    StandardMaxFunding = application.StandardMaxFunding,
                    StandardRoute = application.StandardRoute,
                    NumberOfApprentices = application.NumberOfApprentices,
                    NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
                    StartDate = application.StartDate,
                    Amount = application.Amount,
                    TotalAmount = application.TotalAmount,
                    CurrentFinancialYearAmount = application.CurrentFinancialYearAmount,
                    HasTrainingProvider = application.HasTrainingProvider,
                    Sectors = application.Sectors,
                    FirstName = application.FirstName,
                    LastName = application.LastName,
                    BusinessWebsite = application.BusinessWebsite,
                    EmailAddresses = application.EmailAddresses,
                    CreatedOn = application.CreatedOn,
                    Status = application.Status,
                    IsNamePublic = application.IsNamePublic,
                    Locations = application.Locations?.Select(l => new Application.ApplicationLocation { Id = l.Id, PledgeLocationId = l.PledgeLocationId}),
                    SpecificLocation = application.SpecificLocation,
                    AdditionalLocations = application.AdditionalLocations,
                    SenderEmployerAccountId = application.SenderEmployerAccountId,
                    SenderEmployerAccountName = application.SenderEmployerAccountName,
                    CostProjections = application.CostProjections?.Select(p => new Application.CostProjection{ FinancialYear = p.FinancialYear, Amount = p.Amount}),
                    MatchPercentage = application.MatchPercentage,
                    MatchJobRole = application.MatchJobRole,
                    MatchSector = application.MatchSector,
                    MatchLevel = application.MatchLevel,
                    MatchLocation = application.MatchLocation,
                    CostingModel = application.CostingModel,
                    AutomaticApprovalChoice = application.AutomaticApprovalChoice
                })
            };

            return result;
        }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<Application> Applications { get; set; }
        
        public class Application
        {
            public int Id { get; set; }
            public long EmployerAccountId { get; set; }
            public string DasAccountName { get; set; }
            public int PledgeId { get; set; }
            public string Details { get; set; }
            public string StandardId { get; set; }
            public string StandardTitle { get; set; }
            public int StandardLevel { get; set; }
            public int StandardDuration { get; set; }
            public int StandardMaxFunding { get; set; }
            public string StandardRoute { get; set; }
            public int NumberOfApprentices { get; set; }
            public int NumberOfApprenticesUsed { get; set; }
            public DateTime StartDate { get; set; }
            public int Amount { get; set; }
            public int TotalAmount { get; set; }
            public int CurrentFinancialYearAmount { get; set; }
            public bool HasTrainingProvider { get; set; }
            public IEnumerable<Sector> Sectors { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string BusinessWebsite { get; set; }
            public IEnumerable<string> EmailAddresses { get; set; }
            public DateTime CreatedOn { get; set; }
            public ApplicationStatus Status { get; set; }
            public bool IsNamePublic { get; set; }
            public IEnumerable<ApplicationLocation> Locations { get; set; }
            public string SpecificLocation { get; set; }
            public string AdditionalLocations { get; set; }
            public long SenderEmployerAccountId { get; set; }
            public string SenderEmployerAccountName { get; set; }
            public IEnumerable<CostProjection> CostProjections { get; set; }
            public bool MatchSector { get; set; }
            public bool MatchJobRole { get; set; }
            public bool MatchLevel { get; set; }
            public bool MatchLocation { get; set; }
            public int MatchPercentage { get; set; }
            public string AutomaticApprovalChoice { get; set; }
            public ApplicationCostingModel CostingModel { get; set; }

            public class ApplicationLocation
            {
                public int Id { get; set; }
                public int PledgeLocationId { get; set; }
            }

            public class CostProjection
            {
                public string FinancialYear { get; set; }
                public int Amount { get; set; }
            }
        }
    }
}
