using System;
using NServiceBus;

namespace SFA.DAS.LevyTransferMatching.Messages.Events
{
    public class ApplicationFundingDeclinedEvent : IMessage
    {
        public ApplicationFundingDeclinedEvent(int applicationId, int pledgeId, DateTime declinedOn, int amount)
        {
            Amount = amount;
            ApplicationId = applicationId;
            DeclinedOn = declinedOn;
            PledgeId = pledgeId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime DeclinedOn { get; }
        public int Amount { get; }
    }
}