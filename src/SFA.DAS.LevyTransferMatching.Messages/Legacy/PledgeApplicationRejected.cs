using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_rejected")]
    public class PledgeApplicationRejected
    {
        public PledgeApplicationRejected()
        {

        }

        public PledgeApplicationRejected(int applicationId, int pledgeId, DateTime rejectedOn, int amount, long transferSenderId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            Amount = amount;
            RejectedOn = rejectedOn;
            TransferSenderId = transferSenderId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime RejectedOn { get; }
        public int Amount { get; }
        public long TransferSenderId { get; set; }
    }
}