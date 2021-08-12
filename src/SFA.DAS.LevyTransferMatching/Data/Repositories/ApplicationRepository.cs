using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly LevyTransferMatchingDbContext _dbContext;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public ApplicationRepository(LevyTransferMatchingDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
        {
            _dbContext = dbContext;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task Add(Models.Application application)
        {
            await _dbContext.AddAsync(application);
            await _dbContext.SaveChangesAsync();

            foreach (dynamic domainEvent in application.FlushEvents())
            {
                await _domainEventDispatcher.Send(domainEvent);
            }
        }

        public async Task<Models.Application> Get(int applicationId)
        {
            return await _dbContext.Applications.Include(x => x.EmailAddresses).SingleOrDefaultAsync(x => x.Id == applicationId);
        }
    }
}
