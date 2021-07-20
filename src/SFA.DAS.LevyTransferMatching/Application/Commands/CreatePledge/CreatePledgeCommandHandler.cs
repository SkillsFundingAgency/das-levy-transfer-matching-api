using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using DataModels = SFA.DAS.LevyTransferMatching.Data.Models;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeCommandHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreatePledgeCommandHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            var employerAccount = await _dbContext.EmployerAccounts.FindAsync(command.AccountId);

            var result = await _dbContext.AddAsync(new DataModels.Pledge
            {
                Amount = command.Amount,
                CreatedOn = DateTime.UtcNow,
                EmployerAccount = employerAccount,
                IsNamePublic = command.IsNamePublic,
                Levels = command.Levels.Cast<int>().Sum(),
                JobRoles = command.JobRoles.Cast<int>().Sum(),
                Sectors = command.Sectors.Cast<int>().Sum(),
                PledgeLocations = command.Locations.Select(x =>
                    new DataModels.PledgeLocation
                    {
                        Name = x.Name,
                        Latitude = x.Geopoint[0],
                        Longitude = x.Geopoint[1]
                    }).ToList()
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var pledgeId = result.Entity.Id;

            return new CreatePledgeResult
            {
                Id = pledgeId
            };
        }
    }
}