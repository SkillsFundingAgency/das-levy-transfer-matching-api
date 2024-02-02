using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : PagedQuery, IRequest<GetApplicationsResult>
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
        public long? SenderAccountId { get; set; }
        public ApplicationStatus? ApplicationStatusFilter { get; set; }
        public SortDirection SortDirection { get; set; }
        public GetApplicationsSortOrder SortOrder { get; set; }
    }
}