using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationWithdrawn(int applicationId, int pledgeId, DateTime withdrawnOn) : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public DateTime WithdrawnOn { get; } = withdrawnOn;
}