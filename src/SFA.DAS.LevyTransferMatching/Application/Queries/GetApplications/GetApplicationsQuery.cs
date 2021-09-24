using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsResult>
    {
        public int? PledgeId { get; set; }
        public long? AccountId { get; set; }
    }
}