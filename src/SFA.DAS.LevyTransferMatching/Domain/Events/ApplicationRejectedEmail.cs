using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationRejectedEmail : IDomainEvent
    {
        public ApplicationRejectedEmail(int applicationId, int pledgeId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long ReceiverAccountId { get; }
    }
}
