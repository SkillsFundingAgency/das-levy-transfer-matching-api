using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedReceiverNotificationEvent
    {
        public ApplicationApprovedReceiverNotificationEvent(int applicationId, int pledgeId, long transferSenderId, long transferReceiverId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            TransferSenderId = transferSenderId;
            TransferReceiverId = transferReceiverId;
            ReceiverAccountId = receiverAccountId;
        }

        public ApplicationApprovedReceiverNotificationEvent(int applicationId, int pledgeId, DateTime approvedOn, long transferSenderId, long transferReceiverId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            ApprovedOn = approvedOn;
            TransferSenderId = transferSenderId;
            TransferReceiverId = transferReceiverId;
        }

        public DateTime ApprovedOn { get; }
        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long TransferSenderId { get; set; }
        public long TransferReceiverId { get; set; }
        public long ReceiverAccountId { get; set; }
    }
}
