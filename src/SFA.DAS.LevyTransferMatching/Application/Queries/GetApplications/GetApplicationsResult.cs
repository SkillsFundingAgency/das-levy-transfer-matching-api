using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsResult : List<Models.Application>
    {
        public GetApplicationsResult(IEnumerable<Models.Application> collection) : base(collection)
        {
        }
    }
}