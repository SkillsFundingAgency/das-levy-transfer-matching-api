using System;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events
{
    public class ApplicationApproved : IDomainEvent
    {
        public ApplicationApproved(int applicationId, DateTime approvedOn)
        {
            ApplicationId = applicationId;
            ApprovedOn = approvedOn;
        }

        public int ApplicationId { get; }
        public DateTime ApprovedOn { get; }
    }
}
