using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class PledgeCreditedEvent
    {
        public PledgeCreditedEvent(int pledgeId, long transferSenderId)
        {
            PledgeId = pledgeId;
            TransferSenderId = transferSenderId;
        }

        public int PledgeId { get; set; }
        public long TransferSenderId { get; set; }

    }
}
