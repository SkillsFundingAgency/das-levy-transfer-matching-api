﻿using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_funding_declined")]
    public class PledgeApplicationFundingDeclined
    {
        public PledgeApplicationFundingDeclined()
        {

        }

        public PledgeApplicationFundingDeclined(int applicationId, int pledgeId, DateTime declinedOn, int amount, long accountId)
        {
            Amount = amount;
            ApplicationId = applicationId;
            DeclinedOn = declinedOn;
            PledgeId = pledgeId;
            AccountId = accountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime DeclinedOn { get; }
        public int Amount { get; }
        public long AccountId { get; }
    }
}
