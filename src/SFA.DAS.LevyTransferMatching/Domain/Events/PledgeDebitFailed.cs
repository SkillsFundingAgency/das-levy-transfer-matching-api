using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class PledgeDebitFailed : IDomainEvent
{
    public PledgeDebitFailed(int pledgeId, int applicationId, int amount)
    {
        PledgeId = pledgeId;
        ApplicationId = applicationId;
        Amount = amount;
    }

    public int PledgeId { get; private set; }
    public int ApplicationId { get; private set; }
    public int Amount { get; private set; }
}