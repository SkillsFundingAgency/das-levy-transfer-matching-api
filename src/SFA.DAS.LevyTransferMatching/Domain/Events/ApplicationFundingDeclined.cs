using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Domain.Events;

public class ApplicationFundingDeclined : IDomainEvent
{
    public ApplicationFundingDeclined(int applicationId, int pledgeId, DateTime declinedOn, int amount)
    {
        Amount = amount;
        ApplicationId = applicationId;
        DeclinedOn = declinedOn;
        PledgeId = pledgeId;
    }

    public int ApplicationId { get; }
    public int PledgeId { get; }
    public DateTime DeclinedOn { get; }
    public int Amount { get; }
}