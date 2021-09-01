using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly IApplicationRepository _applicationRepository;

        public GetApplicationQueryHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.Get(request.Id);

            GetApplicationResult result = null;
            if (application != null)
            {
                result = new GetApplicationResult()
                {
                    BusinessWebsite = application.BusinessWebsite,
                    Details = application.Details,
                    EmailAddresses = application.EmailAddresses.Select(x => x.EmailAddress),
                    FirstName = application.FirstName,
                    HasTrainingProvider = application.HasTrainingProvider,
                    LastName = application.LastName,
                    NumberOfApprentices = application.NumberOfApprentices,
                    Postcode = application.Postcode,
                    Sectors = application.Sectors.ToList(),
                    StandardId = application.StandardId,
                    StartDate = application.StartDate,
                    EmployerAccountName = application.EmployerAccount.Name,
                    PledgeLocations = application.Pledge.Locations.ToList(),
                    PledgeSectors = application.Pledge.Sectors.ToList(),
                    PledgeLevels = application.Pledge.Levels.ToList(),
                    PledgeJobRoles = application.Pledge.JobRoles.ToList(),
                    PledgeRemainingAmount = application.Pledge.RemainingAmount
                };
            }

            return result;
        }
    }
}