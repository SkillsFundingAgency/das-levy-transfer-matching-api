using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public class PledgeRepository : IPledgeRepository
{
    private readonly LevyTransferMatchingDbContext _dbContext;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public PledgeRepository(LevyTransferMatchingDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    {
        _dbContext = dbContext;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task Add(Pledge pledge)
    {
        await _dbContext.AddAsync(pledge);
        await _dbContext.SaveChangesAsync();

        foreach (dynamic domainEvent in pledge.FlushEvents())
        {
            await _domainEventDispatcher.Send(domainEvent);
        }
    }

        public async Task<Pledge> Get(int pledgeId)
        {
            return await _dbContext
                .Pledges
                .Include(p => p.Locations)
                .Include(p => p.Applications)
                .SingleOrDefaultAsync(x => x.Id == pledgeId);
        }

    public async Task Update(Pledge pledge)
    {
        foreach (dynamic domainEvent in pledge.FlushEvents())
        {
            await _domainEventDispatcher.Send(domainEvent);
        }
    }
}