using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Models;
using System;
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
            var result = await _dbContext.AddAsync(new PledgeDataModel()
            {
                Amount = pledge.Amount,
                CreationDate = DateTime.UtcNow,
                EmployerAccountId = pledge.AccountId,
                EncodedId = pledge.EncodedAccountId,
                HideEmployerName = !pledge.IsNamePublic,
            });

            await _dbContext.SaveChangesAsync();

            pledge.Id = result.Entity.PledgeId;
        }
    }
}