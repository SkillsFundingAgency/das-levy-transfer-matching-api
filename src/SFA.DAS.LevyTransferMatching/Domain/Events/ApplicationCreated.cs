using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationCreated : IDomainEvent
    {
        public ApplicationCreated(int applicationId, int pledgeId, DateTime createdOn)
        {
            ApplicationId = applicationId;
            CreatedOn = createdOn;
            PledgeId = pledgeId;
        }

        public int ApplicationId { get; }
        public int PledgeId { get; }
        public DateTime CreatedOn { get; }
    }
}
