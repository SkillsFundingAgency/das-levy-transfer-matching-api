using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsResult>
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
        public ApplicationStatus? ApplicationStatusFilter { get; set; }
    }
}