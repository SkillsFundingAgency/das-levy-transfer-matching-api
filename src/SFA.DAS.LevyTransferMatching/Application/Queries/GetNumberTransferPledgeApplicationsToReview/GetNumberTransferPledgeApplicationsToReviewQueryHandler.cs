using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetNumberTransferPledgeApplicationsToReview
{
    public class GetNumberTransferPledgeApplicationsToReviewQueryHandler : IRequestHandler<GetNumberTransferPledgeApplicationsToReviewQuery, GetNumberTransferPledgeApplicationsToReviewQueryResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;


        public GetNumberTransferPledgeApplicationsToReviewQueryHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetNumberTransferPledgeApplicationsToReviewQueryResult> Handle(GetNumberTransferPledgeApplicationsToReviewQuery request, CancellationToken cancellationToken)
        {
            var applications = await _dbContext.Pledges
                                    .Where(x => x.EmployerAccountId == request.TransferSenderId
                                    && x.Status == PledgeStatus.Active)
                                    .SelectMany(x => x.Applications
                                    .Where(app => app.Status == ApplicationStatus.Pending))
                                    .ToListAsync(cancellationToken);

            return new GetNumberTransferPledgeApplicationsToReviewQueryResult
            {
                NumberTransferPledgeApplicationsToReview = applications?.Count() ?? 0
            };
        }
    }
}