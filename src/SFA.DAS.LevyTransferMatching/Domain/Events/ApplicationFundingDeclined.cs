using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationFundingDeclined(int applicationId, int pledgeId, DateTime declinedOn, int amount)
    : IDomainEvent
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public DateTime DeclinedOn { get; } = declinedOn;
    public int Amount { get; } = amount;
}