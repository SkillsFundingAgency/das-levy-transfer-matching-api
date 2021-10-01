using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class AcceptedFunding : IDomainEvent
    {
        public AcceptedFunding(int applicationId, long accountId, DateTime acceptedFundingOn) {
            ApplicationId = applicationId;
            AcceptedFundingOn = acceptedFundingOn;
            AccountId = accountId;
        }

        public int ApplicationId { get; }
        public long AccountId { get; }
        public DateTime AcceptedFundingOn { get; }
    }
}
