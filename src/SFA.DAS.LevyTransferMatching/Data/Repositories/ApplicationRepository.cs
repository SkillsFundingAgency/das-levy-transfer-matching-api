using System.Linq;
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

        public async Task Update(Models.Application application)
        {
            foreach (dynamic domainEvent in application.FlushEvents())
            {
                await _domainEventDispatcher.Send(domainEvent);
            }
        }

        public async Task<Models.Application> Get(int? pledgeId, int applicationId, long? accountId)
        {
            var query = _dbContext.Applications
                .Include(o => o.EmployerAccount)
                .Where(x => x.Id == applicationId);

            if (pledgeId.HasValue)
            { 
                query = query.Where(x => x.Pledge.Id == pledgeId);
            }

            if (accountId.HasValue)
            {
                query = query.Where(x => x.EmployerAccount.Id == accountId);
            }

            return await query.SingleOrDefaultAsync();
        }
    }
}
