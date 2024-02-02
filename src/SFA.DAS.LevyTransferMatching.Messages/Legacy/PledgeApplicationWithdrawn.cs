using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_withdrawn")]
    public class PledgeApplicationWithdrawn
    {
        public PledgeApplicationWithdrawn()
        {

        }

        public PledgeApplicationWithdrawn(int applicationId, int pledgeId, DateTime withdrawnOn, long transferSenderId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            WithdrawnOn = withdrawnOn;
            TransferSenderId = transferSenderId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime WithdrawnOn { get; }
        public long TransferSenderId { get; set; }
    }
}