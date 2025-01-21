using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsToAutoDecline;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications;

public class GetApplicationsToAutoDeclineResponse
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }

    public static implicit operator GetApplicationsToAutoDeclineResponse(GetApplicationsToAutoDeclineResult source)
    {
        return new GetApplicationsToAutoDeclineResponse
        {
            ApplicationIdsToDecline = source.ApplicationIdsToDecline
        };
    }
}
