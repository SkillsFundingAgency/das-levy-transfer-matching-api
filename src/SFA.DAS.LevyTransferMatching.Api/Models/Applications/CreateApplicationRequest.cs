using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Api.Models.Base;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class CreateApplicationRequest : StateChangeRequest
    {
        public long EmployerAccountId { get; set; }

        public string Details { get; set; }

        public string StandardId { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public int StandardDuration { get; set; }
        public int StandardMaxFunding { get; set; }
        public string StandardRoute { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
        public List<int> Locations { get; set; }
        public string AdditionalLocation { get; set; }
        public string SpecificLocation { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
    }
}
