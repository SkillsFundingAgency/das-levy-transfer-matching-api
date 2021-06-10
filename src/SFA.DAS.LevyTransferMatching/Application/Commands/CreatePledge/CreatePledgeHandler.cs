using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using DataModels = SFA.DAS.LevyTransferMatching.Data.Models;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public CreatePledgeHandler(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            var result = await _dbContext.AddAsync(new DataModels.Pledge()
            {
                Amount = command.Amount,
                CreationDate = DateTime.UtcNow,
                EmployerAccountId = command.AccountId,
                IsNamePublic = command.IsNamePublic,
                PledgeLevels = command.Levels.Select(x => new DataModels.PledgeLevel() { LevelId = (byte)x }).ToList(),
                PledgeRoles = command.JobRoles.Select(x => new DataModels.PledgeRole() { RoleId = (byte)x }).ToList(),
                PledgeSectors = command.Sectors.Select(x => new DataModels.PledgeSector() { SectorId = (byte)x }).ToList(),
            });

            await _dbContext.SaveChangesAsync();

            command.Id = result.Entity.PledgeId;

            return new CreatePledgeResult()
            {
                Id = command.Id.Value,
            };
        }
    }
}