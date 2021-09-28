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
            var application = await _levyTransferMatchingDbContext.Applications
                .Include(x => x.ApplicationLocations)
                .Include(x => x.EmailAddresses)
                .Include(x => x.EmployerAccount)
                .Include(x => x.Pledge)
                .Include(x => x.Pledge.Locations)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (application == null)
            {
                return null;
            }

            var result = new GetApplicationResult()
            {
                BusinessWebsite = application.BusinessWebsite,
                Details = application.Details,
                EmailAddresses = application.EmailAddresses.Select(x => x.EmailAddress),
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                LastName = application.LastName,
                NumberOfApprentices = application.NumberOfApprentices,
                Postcode = "",
                Sectors = application.Sectors.ToList(),
                StandardId = application.StandardId,
                StartDate = application.StartDate,
                EmployerAccountName = application.EmployerAccount.Name,
                Locations = application.ApplicationLocations.Select(x => new GetApplicationResult.ApplicationLocation { Id = x.Id, PledgeLocationId = x.PledgeLocationId }).ToList(),
                AdditionalLocation = application.AdditionalLocation,
                SpecificLocation = application.SpecificLocation,
                PledgeLocations = application.Pledge.Locations.ToList(),
                PledgeSectors = application.Pledge.Sectors.ToList(),
                PledgeLevels = application.Pledge.Levels.ToList(),
                PledgeJobRoles = application.Pledge.JobRoles.ToList(),
                Amount = application.Amount,
                PledgeRemainingAmount = application.Pledge.RemainingAmount,
                Status = application.Status
            };

            return result;
        }
    }
}