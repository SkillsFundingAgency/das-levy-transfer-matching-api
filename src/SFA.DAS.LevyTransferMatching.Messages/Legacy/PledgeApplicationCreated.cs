using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Legacy
{
    [MessageGroup("pledge_application_created")]
    public class PledgeApplicationCreated
    {
        public PledgeApplicationCreated()
        {
        }

        public PledgeApplicationCreated(int applicationId, int pledgeId, DateTime createdOn, long transferSenderId)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            CreatedOn = createdOn;
            TransferSenderId = transferSenderId;
        }

        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long TransferSenderId { get; set; }
    }
}
