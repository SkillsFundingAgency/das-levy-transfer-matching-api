using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories
{
    public class PledgeRepository : IPledgeRepository
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public PledgeRepository(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Pledge pledge)
        {
            await _dbContext.AddAsync(pledge);
        }

        public async Task<Pledge> Get(int pledgeId)
        {
            return await _dbContext.Pledges.FindAsync(pledgeId);
        }
    }

    public interface IPledgeRepository
    {
        Task Add(Pledge pledge);
        Task<Pledge> Get(int pledgeId);
    }
}
