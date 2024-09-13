using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationWithdrawnAfterAcceptanceEvent : IMessage
    {
        public ApplicationWithdrawnAfterAcceptanceEvent(int applicationId, int pledgeId, int amount)
        {
            ApplicationId = applicationId;
            PledgeId = pledgeId;
            Amount = amount;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public int Amount { get; }
    }
}
