using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedEmailEvent
    {
        public ApplicationApprovedEmailEvent(int applicationId, int pledgeId, long transferSenderId, long transferReceiverId, long receiverAccountId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            TransferSenderId = transferSenderId;
            TransferReceiverId = transferReceiverId;
            ReceiverAccountId = receiverAccountId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public long TransferSenderId { get; set; }
        public long TransferReceiverId { get; set; }
        public long ReceiverAccountId { get; set; }
    }
}
