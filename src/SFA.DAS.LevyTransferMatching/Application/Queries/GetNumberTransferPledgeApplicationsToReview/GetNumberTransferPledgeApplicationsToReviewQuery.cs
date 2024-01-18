using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetNumberTransferPledgeApplicationsToReview
{
    public class GetNumberTransferPledgeApplicationsToReviewQuery : IRequest<GetNumberTransferPledgeApplicationsToReviewQueryResult>
    {
        public long TransferSenderId { get; set; }
    }
}