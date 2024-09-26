using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public interface IPledgeRepository
{
    Task Add(Pledge pledge);
    Task<Pledge> Get(int pledgeId);
    Task Update(Pledge pledge);
}

public class PledgeRepository(LevyTransferMatchingDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    : IPledgeRepository
{
    public async Task Add(Pledge pledge)
    {
        await dbContext.AddAsync(pledge);
        await dbContext.SaveChangesAsync();

        foreach (dynamic domainEvent in pledge.FlushEvents())
        {
            await domainEventDispatcher.Send(domainEvent);
        }
    }

        public async Task<Pledge> Get(int pledgeId)
        {
            return await dbContext
                .Pledges
                .Include(p => p.Locations)
                .Include(p => p.Applications)
                .SingleOrDefaultAsync(x => x.Id == pledgeId);
        }

    public async Task Update(Pledge pledge)
    {
        foreach (dynamic domainEvent in pledge.FlushEvents())
        {
            await domainEventDispatcher.Send(domainEvent);
        }
    }
}