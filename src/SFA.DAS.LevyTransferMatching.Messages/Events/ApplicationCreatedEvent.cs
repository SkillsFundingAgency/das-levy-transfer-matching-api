using System;
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationCreatedEvent : IMessage
    {
        public ApplicationCreatedEvent(int applicationId, int pledgeId, DateTime createdOn, long transferSenderId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            CreatedOn = createdOn;
            TransferSenderId = transferSenderId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime CreatedOn { get; }
        public long TransferSenderId { get; set; }
        public long ReceiverAccountId { get; set; }

    }
}
