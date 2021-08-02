using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeCommandHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly IEmployerAccountRepository _employerAccountRepository;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreatePledgeCommandHandler(IEmployerAccountRepository employerAccountRepository,
            IPledgeRepository pledgeRepository,
            LevyTransferMatchingDbContext dbContext)
        {
            _employerAccountRepository = employerAccountRepository;
            _pledgeRepository = pledgeRepository;
            _dbContext = dbContext;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            var employerAccount = await _employerAccountRepository.Get(request.AccountId);

            var pledge = employerAccount.CreatePledge(request.Amount,
                request.IsNamePublic,
                (Level)request.Levels.Cast<int>().Sum(),
                (JobRole)request.JobRoles.Cast<int>().Sum(),
                (Sector)request.Sectors.Cast<int>().Sum(),
                request.Locations.Select(x =>
                new PledgeLocation
                {
                    Name = x.Name,
                    Latitude = x.Geopoint[0],
                    Longitude = x.Geopoint[1]
                }).ToList()
            );

            await _pledgeRepository.Add(pledge);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var pledgeId = pledge.Id;

            return new CreatePledgeResult
            {
                Id = pledgeId
            };
        }
    }
}