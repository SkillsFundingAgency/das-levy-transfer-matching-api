using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using System;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationFundingAccepted : IDomainEvent
    {
        public ApplicationFundingAccepted(int applicationId, int pledgeId, long accountId, DateTime acceptedOn)
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
