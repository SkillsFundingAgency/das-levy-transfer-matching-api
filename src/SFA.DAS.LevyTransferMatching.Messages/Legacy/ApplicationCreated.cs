using System;
using SFA.DAS.Messaging.Attributes;

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

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime CreatedOn { get; }
        public long TransferSenderId { get; set; }
    }
}
