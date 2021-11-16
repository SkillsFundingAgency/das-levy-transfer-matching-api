using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using System;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationRejected : IDomainEvent
    {
        public ApplicationRejected(int applicationId, int pledgeId, DateTime rejectedOn, int amount)
        {
            Amount = amount;
            ApplicationId = applicationId;
            RejectedOn = rejectedOn;
            PledgeId = pledgeId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime RejectedOn { get; }
        public int Amount { get; }
    }
}
