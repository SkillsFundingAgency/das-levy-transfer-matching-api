using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class GetApplicationsRequest
    {
        [FromQuery]
        public int? PledgeId { get; set; }
        [FromQuery]
        public long? AccountId { get; set; }
        [FromQuery]
        public long? SenderAccountId { get; set; }
        [FromQuery]
        public ApplicationStatus? ApplicationStatusFilter { get; set; }
        [FromQuery]
        public int Page { get; set; }
        [FromQuery]
        public int? PageSize { get; set; }
        [FromQuery]
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
        [FromQuery]
        public GetApplicationsSortOrder SortOrder { get; set; } = GetApplicationsSortOrder.ApplicationDate;
    }
}
