using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
    {
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IEmployerAccountRepository _employerAccountRepository;
        private readonly IApplicationRepository _applicationRepository;

        public CreateApplicationCommandHandler(IPledgeRepository pledgeRepository,
            IApplicationRepository applicationRepository,
            IEmployerAccountRepository employerAccountRepository)
        {
            _pledgeRepository = pledgeRepository;
            _applicationRepository = applicationRepository;
            _employerAccountRepository = employerAccountRepository;
        }

        public async Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var account = await _employerAccountRepository.Get(request.EmployerAccountId);
            var pledge = await _pledgeRepository.Get(request.PledgeId);

            var settings = new CreateApplicationProperties
            {
                Details = request.Details,
                StandardId = request.StandardId,
                NumberOfApprentices = request.NumberOfApprentices,
                StartDate = request.StartDate,
                HasTrainingProvider = request.HasTrainingProvider,
                Amount = request.Amount,
                Sectors = (Sector) request.Sectors.Cast<int>().Sum(),
                Locations = request.Locations,
                AdditionalLocation = request.AdditionalLocation,
                SpecificLocation = request.SpecificLocation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BusinessWebsite = request.BusinessWebsite,
                EmailAddresses = request.EmailAddresses
            };

            var application = pledge.CreateApplication(account, settings, new UserInfo(request.UserId, request.UserDisplayName));

            await _applicationRepository.Add(application);
           
            return new CreateApplicationCommandResult
            {
                ApplicationId = application.Id
            };
        }
    }
}