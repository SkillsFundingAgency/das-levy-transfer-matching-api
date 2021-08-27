using System;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class ApplicationStatusHistory : Entity<int>
    {
        public ApplicationStatusHistory(ApplicationStatus status, DateTime createdOn)
        {
            Status = status;
            CreatedOn = createdOn;
        }

        public int ApplicationId { get; protected set; }

        public DateTime CreatedOn { get; protected set; }
        public ApplicationStatus Status { get; protected set; }
    }
}
