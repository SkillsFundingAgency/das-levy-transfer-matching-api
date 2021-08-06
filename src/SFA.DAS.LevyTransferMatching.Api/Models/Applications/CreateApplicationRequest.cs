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
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasTrainingProvider { get; set; }
        public int Amount { get; set; }

        public IEnumerable<Sector> Sectors { get; set; }
        public string Postcode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public string BusinessWebsite { get; set; }
    }
}
