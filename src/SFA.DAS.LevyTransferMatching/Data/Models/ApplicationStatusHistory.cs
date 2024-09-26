using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Data.Models;

public class ApplicationStatusHistory(ApplicationStatus status, DateTime createdOn) : Entity<int>
{
    public int ApplicationId { get; protected set; }

    public DateTime CreatedOn { get; protected set; } = createdOn;
    public ApplicationStatus Status { get; protected set; } = status;
}