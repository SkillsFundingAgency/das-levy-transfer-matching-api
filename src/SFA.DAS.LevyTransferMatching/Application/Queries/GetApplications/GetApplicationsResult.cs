using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsResult : PagedQueryResult<GetApplicationsResult.Application>
    {
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
            public List<ApplicationLocation> Locations { get; set; }
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