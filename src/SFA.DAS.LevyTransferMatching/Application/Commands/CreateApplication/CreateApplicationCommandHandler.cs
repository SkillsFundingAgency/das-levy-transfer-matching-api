using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
    {
        private readonly IPledgeRepository _pledgeRepository;
        private readonly IEmployerAccountRepository _employerAccountRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreateApplicationCommandHandler(IPledgeRepository pledgeRepository,
            IApplicationRepository applicationRepository,
            IEmployerAccountRepository employerAccountRepository,
            LevyTransferMatchingDbContext dbContext)
        {
            _pledgeRepository = pledgeRepository;
            _applicationRepository = applicationRepository;
            _employerAccountRepository = employerAccountRepository;
            _dbContext = dbContext;
        }

        public async Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var accountTask = _employerAccountRepository.Get(request.EmployerAccountId);
            var pledgeTask = _pledgeRepository.Get(request.PledgeId);

            await Task.WhenAll(accountTask, pledgeTask);

            var account = accountTask.Result;
            var pledge = pledgeTask.Result;

            var application = pledge.CreateApplication(account,
                request.Details,
                request.StandardId,
                request.NumberOfApprentices,
                request.StartDate,
                request.HasTrainingProvider,
                (Sector) request.Sectors.Cast<int>().Sum(),
                request.Postcode,
                request.FirstName,
                request.LastName,
                request.BusinessWebsite,
                request.EmailAddresses);

            await _applicationRepository.Add(application);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateApplicationCommandResult
            {
                ApplicationId = application.Id
            };
        }
    }
}