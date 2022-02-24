using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : PagedQuery, IRequest<GetApplicationsResult>
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
        public ApplicationStatus? ApplicationStatusFilter { get; set; }
    }
}