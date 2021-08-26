using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationApproved : IDomainEvent
    {
        public ApplicationApproved(int applicationId, int pledgeId, DateTime approvedOn, int amount)
        {
            Amount = amount;
            ApplicationId = applicationId;
            ApprovedOn = approvedOn;
            PledgeId = pledgeId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime ApprovedOn { get; }
        public int Amount { get; }
    }
}
