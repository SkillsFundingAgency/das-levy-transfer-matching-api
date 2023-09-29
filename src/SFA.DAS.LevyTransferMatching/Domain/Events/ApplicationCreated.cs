using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationCreated : IDomainEvent
    {
        public ApplicationCreated(int applicationId, int pledgeId, long receiverAccountId, DateTime createdOn)
        {
            ApplicationId = applicationId;
            CreatedOn = createdOn;
            PledgeId = pledgeId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long ReceiverAccountId { get; set; }

        public DateTime CreatedOn { get; }
    }
}
