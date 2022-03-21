﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_funding_accepted")]
    public class PledgeApplicationApplicationFundingAccepted
    {
        public PledgeApplicationApplicationFundingAccepted(int applicationId, int pledgeId, long accountId, DateTime acceptedOn)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            AccountId = accountId;
            AcceptedOn = acceptedOn;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long AccountId { get; }
        public DateTime AcceptedOn { get; }
    }
}
