
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class PledgeClosedEvent : IMessage
    {
        public PledgeClosedEvent(int pledgeId, bool insufficientFunds)
        {
            PledgeId = pledgeId;
            InsufficientFunds = insufficientFunds;
        }

        public int PledgeId { get; set; }
        public bool InsufficientFunds { get; set; }
    }
}