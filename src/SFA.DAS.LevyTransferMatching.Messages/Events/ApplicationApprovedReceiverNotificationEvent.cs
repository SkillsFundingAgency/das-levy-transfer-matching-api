using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedReceiverNotificationEvent
    {
        public ApplicationApprovedReceiverNotificationEvent(int applicationId, int pledgeId, DateTime approvedOn, long transferSenderId, long transferReceiverId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            ApprovedOn = approvedOn;
            TransferSenderId = transferSenderId;
            TransferReceiverId = transferReceiverId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime ApprovedOn { get; }
        public long TransferSenderId { get; set; }
        public long TransferReceiverId { get; set; }
    }
}
