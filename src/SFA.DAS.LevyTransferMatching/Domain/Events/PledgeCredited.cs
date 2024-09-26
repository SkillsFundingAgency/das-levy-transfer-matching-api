using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class PledgeCredited(int pledgeId) : IDomainEvent
{
    public int PledgeId { get; set; } = pledgeId;
}