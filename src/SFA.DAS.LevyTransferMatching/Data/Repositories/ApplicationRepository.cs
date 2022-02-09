using System;
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

        public async Task<Models.Application> Get(int applicationId, int? pledgeId = null, long? accountId = null)
        {
            var application = await _dbContext.Applications
                .Include(o => o.EmployerAccount)
                .Include(o => o.ApplicationCostProjections)
                .Where(x => x.Id == applicationId)
                .SingleOrDefaultAsync();

            if (pledgeId.HasValue && pledgeId != application.PledgeId)
            {
                throw new InvalidOperationException("The application's pledge Id does not match the provided pledgeId argument");
            }

            if (accountId.HasValue && accountId != application.EmployerAccount.Id)
            {
                throw new InvalidOperationException("The application's account Id does not match the provided accountId argument");
            }

            return application;
        }
    }
}
