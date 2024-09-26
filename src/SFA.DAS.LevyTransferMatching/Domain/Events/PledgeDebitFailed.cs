using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class PledgeDebitFailed(int pledgeId, int applicationId, int amount) : IDomainEvent
{
    public int PledgeId { get; private set; } = pledgeId;
    public int ApplicationId { get; private set; } = applicationId;
    public int Amount { get; private set; } = amount;
}