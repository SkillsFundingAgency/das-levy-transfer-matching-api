using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedEvent
    {
        public ApplicationApprovedEvent(int applicationId, int pledgeId, DateTime approvedOn, int amount, long transferSenderId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            Amount = amount;
            ApprovedOn = approvedOn;
            TransferSenderId = transferSenderId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime ApprovedOn { get; }
        public int Amount { get; }
        public long TransferSenderId { get; set; }
    }
}
