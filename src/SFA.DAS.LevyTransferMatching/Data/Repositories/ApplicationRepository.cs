using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly LevyTransferMatchingDbContext _dbContext;

        public ApplicationRepository(LevyTransferMatchingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Models.Application application)
        {
            await _dbContext.AddAsync(application);
        }
    }
}
