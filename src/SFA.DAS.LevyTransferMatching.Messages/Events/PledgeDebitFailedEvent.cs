using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class PledgeDebitFailedEvent : IMessage
    {
        public PledgeDebitFailedEvent(int pledgeId, int applicationId, int amount)
        {
            PledgeId = pledgeId;
            ApplicationId = applicationId;
            Amount = amount;
        }

        public int PledgeId { get; private set; }
        public int ApplicationId { get; private set; }
        public int Amount { get; private set; }
    }
}
