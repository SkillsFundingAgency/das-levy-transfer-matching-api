using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedEmailEvent
    {
        public ApplicationApprovedEmailEvent(int applicationId, int pledgeId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }      
        public long ReceiverAccountId { get; set; }
    }
}
