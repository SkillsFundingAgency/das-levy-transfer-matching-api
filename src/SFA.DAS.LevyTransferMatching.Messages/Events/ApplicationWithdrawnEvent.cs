using System;
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationWithdrawnEvent : IMessage
    {
        public ApplicationWithdrawnEvent(int applicationId, int pledgeId, DateTime withdrawnOn, long transferSenderId)
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
