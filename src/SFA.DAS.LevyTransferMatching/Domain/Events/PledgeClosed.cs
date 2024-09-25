using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class PledgeClosed(int pledgeId, bool insufficientFunds) : IDomainEvent
{
    public int PledgeId { get; set; } = pledgeId;
    public bool InsufficientFunds { get; set; } = insufficientFunds;
}