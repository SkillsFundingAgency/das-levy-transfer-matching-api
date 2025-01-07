namespace SFA.DAS.LevyTransferMatching.Messages.Events;

public class ApplicationFundingExpiredEvent(int applicationId)
{
    public int ApplicationId { get; } = applicationId;
}
