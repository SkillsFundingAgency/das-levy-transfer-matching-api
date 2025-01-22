namespace SFA.DAS.LevyTransferMatching.Messages.Events;

public class ApplicationFundingExpiredEvent(int applicationId, int pledgeId, int amount)
{
    public int ApplicationId { get; } = applicationId;
    public int PledgeId { get; } = pledgeId;
    public int Amount { get; } = amount;
}
