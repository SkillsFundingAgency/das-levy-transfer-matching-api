using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationWithdrawnAfterAcceptance(int applicationId, int pledgeId, int amount) : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public int Amount { get; } = amount;
}