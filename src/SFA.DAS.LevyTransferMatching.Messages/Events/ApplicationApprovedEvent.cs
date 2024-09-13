using System;
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedEvent : IMessage
    {
        public ApplicationApprovedEvent(int applicationId, int pledgeId, DateTime approvedOn, int amount, long transferSenderId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            Amount = amount;
            ApprovedOn = approvedOn;
            TransferSenderId = transferSenderId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime ApprovedOn { get; }
        public int Amount { get; }
        public long TransferSenderId { get; set; }
        public long ReceiverAccountId { get; set; }

    }
}
