using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationFundingAcceptedEvent
    {
        public ApplicationFundingAcceptedEvent(int applicationId, int pledgeId, long accountId, DateTime acceptedOn)
        {
            ApplicationId = applicationId;
            AcceptedOn = acceptedOn;
            AccountId = accountId;
            PledgeId = pledgeId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long AccountId { get; }
        public DateTime AcceptedOn { get; }
    }
}
