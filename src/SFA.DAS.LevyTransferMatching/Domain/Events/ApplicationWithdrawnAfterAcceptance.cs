using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using System;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationWithdrawnAfterAcceptance : IDomainEvent
    {
        public ApplicationWithdrawnAfterAcceptance(int applicationId, int pledgeId, int amount)
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
