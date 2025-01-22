namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoExpire;

public class GetApplicationsToAutoExpireResult
{
    public IEnumerable<int> ApplicationIdsToExpire { get; set; }
}