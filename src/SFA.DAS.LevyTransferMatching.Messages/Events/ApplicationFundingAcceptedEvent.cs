using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationFundingAcceptedEvent
    {
        public ApplicationFundingAcceptedEvent(int applicationId, int pledgeId, bool rejectApplications)
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
