using System;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationApprovedEvent
    {
        public ApplicationApprovedEvent(int applicationId, DateTime approvedOn, int amount)
        {
            Amount = amount;
            ApplicationId = applicationId;
            ApprovedOn = approvedOn;
        }

        public int ApplicationId { get; }
        public DateTime ApprovedOn { get; }
        public int Amount { get; }
    }
}
