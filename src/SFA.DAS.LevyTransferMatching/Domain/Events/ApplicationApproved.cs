using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationApproved : IDomainEvent
    {
        public ApplicationApproved(int applicationId, int pledgeId, DateTime approvedOn, int amount, long receiverAccountId)
        {
            Amount = amount;
            ApplicationId = applicationId;
            ApprovedOn = approvedOn;
            PledgeId = pledgeId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime ApprovedOn { get; }
        public int Amount { get; }
        public long ReceiverAccountId { get; }
    }
}
