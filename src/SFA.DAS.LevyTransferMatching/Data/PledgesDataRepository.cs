using SFA.DAS.LevyTransferMatching.Models;
using DataModels = SFA.DAS.LevyTransferMatching.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public class PledgesDataRepository : IPledgesDataRepository
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public PledgesDataRepository(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Pledge pledge)
        {
            var result = await _dbContext.AddAsync(new DataModels.Pledge()
            {
                Amount = pledge.Amount,
                CreationDate = DateTime.UtcNow,
                EmployerAccountId = pledge.AccountId,
                EncodedId = pledge.EncodedAccountId,
                HideEmployerName = !pledge.IsNamePublic,
                PledgeLevels = pledge.Levels.Select(x => new DataModels.PledgeLevel() { LevelId = (byte)x }).ToList(),
                PledgeRoles = pledge.JobRoles.Select(x => new DataModels.PledgeRole() { RoleId = (byte)x }).ToList(),
                PledgeSectors = pledge.Sectors.Select(x => new DataModels.PledgeSector() { SectorId = (byte)x }).ToList(),
            });

            await _dbContext.SaveChangesAsync();

            pledge.Id = result.Entity.PledgeId;
        }
    }
}