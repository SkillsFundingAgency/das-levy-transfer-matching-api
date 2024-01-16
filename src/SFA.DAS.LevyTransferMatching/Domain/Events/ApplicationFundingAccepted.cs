using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationFundingAccepted : IDomainEvent
    {
        public ApplicationFundingAccepted(int applicationId, int pledgeId, bool rejectApplications)
        {            
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            RejectApplications = rejectApplications;
        }

        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }      
        public bool RejectApplications { get; set; }
    }
}
