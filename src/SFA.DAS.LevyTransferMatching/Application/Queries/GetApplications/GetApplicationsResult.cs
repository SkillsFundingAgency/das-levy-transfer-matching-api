using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsResult
    {
        public IEnumerable<Models.Application> Applications { get; set; }

        public GetApplicationsResult(IEnumerable<Models.Application> applications)
        {
            Applications = applications;
        }
    }
}