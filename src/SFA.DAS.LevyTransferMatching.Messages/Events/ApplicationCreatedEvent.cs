using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationCreatedEvent
    {
        public ApplicationCreatedEvent(int applicationId, int pledgeId, DateTime createdOn, long transferSenderId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            CreatedOn = createdOn;
            TransferSenderId = transferSenderId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime CreatedOn { get; }
        public long TransferSenderId { get; set; }
    }
}
