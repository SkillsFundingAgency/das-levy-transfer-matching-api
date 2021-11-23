using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly LevyTransferMatchingDbContext _levyTransferMatchingDbContext;

        public GetApplicationQueryHandler(LevyTransferMatchingDbContext levyTransferMatchingDbContext)
        {
            _levyTransferMatchingDbContext = levyTransferMatchingDbContext;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var applicationQuery = _levyTransferMatchingDbContext.Applications
                .Include(x => x.ApplicationLocations)
                .Include(x => x.EmailAddresses)
                .Include(x => x.EmployerAccount)
                .Include(x => x.Pledge)
                .Include(x => x.Pledge.EmployerAccount)
                .Include(x => x.Pledge.Locations)
                .Where(x => x.Id == request.ApplicationId)
                .AsQueryable();

            if (request.PledgeId.HasValue)
            {
                applicationQuery = applicationQuery
                    .Where(x => x.PledgeId == request.PledgeId.Value);
            }
            
            var application = await applicationQuery.SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (application == null)
            {
                return null;
            }

            var result = new GetApplicationResult
            {
                BusinessWebsite = application.BusinessWebsite,
                Details = application.Details,
                EmailAddresses = application.EmailAddresses.Select(x => x.EmailAddress),
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                LastName = application.LastName,
                NumberOfApprentices = application.NumberOfApprentices,
                Sectors = application.Sectors.ToList(),
                StandardId = application.StandardId,
                StandardTitle = application.StandardTitle,
                StandardLevel = application.StandardLevel,
                StandardDuration = application.StandardDuration,
                StandardMaxFunding = application.StandardMaxFunding,
                StandardRoute = application.StandardRoute,
                StartDate = application.StartDate,
                EmployerAccountName = application.EmployerAccount.Name,
                AmountUsed = application.AmountUsed,
                NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
                PledgeEmployerAccountName = application.Pledge.EmployerAccount.Name,
                Locations = application.ApplicationLocations.Select(x => new GetApplicationResult.ApplicationLocation { Id = x.Id, PledgeLocationId = x.PledgeLocationId }).ToList(),
                AdditionalLocation = application.AdditionalLocation,
                SpecificLocation = application.SpecificLocation,
                PledgeLocations = application.Pledge.Locations.ToList(),
                PledgeSectors = application.Pledge.Sectors.ToList(),
                PledgeLevels = application.Pledge.Levels.ToList(),
                PledgeJobRoles = application.Pledge.JobRoles.ToList(),
                Amount = application.Amount,
                TotalAmount = application.TotalAmount,
                PledgeAmount = application.Pledge.Amount,
                PledgeRemainingAmount = application.Pledge.RemainingAmount,
                Status = application.Status,
                PledgeIsNamePublic = application.Pledge.IsNamePublic,
                PledgeId = application.PledgeId,
                ReceiverEmployerAccountId = application.EmployerAccount.Id,
                SenderEmployerAccountId = application.Pledge.EmployerAccount.Id,
                AutomaticApproval = application.AutomaticApproval
            };

            return result;
        }
    }
}