using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_funding_declined")]
    public class PledgeApplicationFundingDeclined
    {
        public PledgeApplicationFundingDeclined()
        {

        }

        public PledgeApplicationFundingDeclined(int applicationId, int pledgeId, DateTime declinedOn, long accountId)
        {
            ApplicationId = applicationId;
            DeclinedOn = declinedOn;
            PledgeId = pledgeId;
            AccountId = accountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime DeclinedOn { get; }
        public long AccountId { get; }
    }
}
