using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsForAutomaticApproval;

public class GetApplicationsForAutomaticApprovalQuery : IRequest<GetApplicationsForAutomaticApprovalResult>
{
    public int? PledgeId { get; set; }
}
