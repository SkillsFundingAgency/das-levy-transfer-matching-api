using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoExpire;

namespace SFA.DAS.LevyTransferMatching.Api.Models.GetApplicationsToAutoExpire;

public class GetApplicationsToAutoExpireResponse
{
    public IEnumerable<int> ApplicationsToExpire { get; set; }

    public static implicit operator GetApplicationsToAutoExpireResponse(GetApplicationsToAutoExpireResult source)
    {
        return new GetApplicationsToAutoExpireResponse
        {
            ApplicationsToExpire = source.ApplicationsToExpire
        };
    }
}
