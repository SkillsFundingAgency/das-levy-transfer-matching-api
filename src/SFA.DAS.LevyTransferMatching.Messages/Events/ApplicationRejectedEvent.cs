using System;
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationRejectedEvent : IMessage
    {
        public ApplicationRejectedEvent(int applicationId, int pledgeId, DateTime rejectedOn, int amount, long transferSenderId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            Amount = amount;
            RejectedOn = rejectedOn;
            TransferSenderId = transferSenderId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime RejectedOn { get; }
        public int Amount { get; }
        public long TransferSenderId { get; set; }
        public long ReceiverAccountId { get; set; }

    }
}
