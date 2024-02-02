using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class PledgeClosed : IDomainEvent
{
    public PledgeClosed(int pledgeId, bool insufficientFunds)
    {
        PledgeId = pledgeId;
        InsufficientFunds = insufficientFunds;
    }

    public int PledgeId { get; set; }
    public bool InsufficientFunds { get; set; }
}