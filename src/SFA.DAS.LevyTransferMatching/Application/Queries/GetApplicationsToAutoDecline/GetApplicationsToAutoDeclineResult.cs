namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

public class GetApplicationsToAutoDeclineResult
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }
}
