using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

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
            var account = await _employerAccountRepository.Get(request.EmployerAccountId);
            var pledge = await _pledgeRepository.Get(request.PledgeId);

            var application = pledge.CreateApplication(account);

            await _applicationRepository.Add(application);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateApplicationCommandResult
            {
                ApplicationId = application.Id
            };
        }
    }
}