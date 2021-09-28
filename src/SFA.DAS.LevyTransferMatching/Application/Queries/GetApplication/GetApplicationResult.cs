using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationResult
    {
        public string Postcode { get; set; }
        public string StandardId { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public int Amount { get; set; }
        public bool HasTrainingProvider { get; set; }
        public string Details { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
        public string EmployerAccountName { get; set; }
        public List<ApplicationLocation> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }
        public List<PledgeLocation> PledgeLocations { get; set; }
        public ApplicationStatus Status { get; set; }
        public IEnumerable<Sector> PledgeSectors { get; set; }
        public IEnumerable<Level> PledgeLevels { get; set; }
        public IEnumerable<JobRole> PledgeJobRoles { get; set; }
        public int PledgeRemainingAmount { get; set; }

        public class ApplicationLocation
        {
            public int Id { get; set; }
            public int PledgeLocationId { get; set; }
        }
    }
}