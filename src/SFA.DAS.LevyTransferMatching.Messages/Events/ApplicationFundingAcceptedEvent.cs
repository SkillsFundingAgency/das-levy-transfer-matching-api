using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationFundingAcceptedEvent : IMessage
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
