using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public interface IApplicationRepository
{
    Task Add(Models.Application application);
    Task Update(Models.Application application);
    Task<Models.Application> Get(int applicationId, int? pledgeId = null, long? accountId = null);
}

public class ApplicationRepository(LevyTransferMatchingDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
    : IApplicationRepository
{
    public async Task Add(Models.Application application)
    {
        await dbContext.AddAsync(application);
        await dbContext.SaveChangesAsync();

        foreach (dynamic domainEvent in application.FlushEvents())
        {
            await domainEventDispatcher.Send(domainEvent);
        }
    }

    public async Task Update(Models.Application application)
    {
        await dbContext.SaveChangesAsync();

        foreach (dynamic domainEvent in application.FlushEvents())
        {
            await domainEventDispatcher.Send(domainEvent);
        }
    }

    public async Task<Models.Application> Get(int applicationId, int? pledgeId = null, long? accountId = null)
    {
        var application = await dbContext.Applications
            .Include(o => o.EmployerAccount)
            .Include(o => o.ApplicationLocations)
            .Include(o => o.EmailAddresses)
            .Include(o => o.ApplicationCostProjections)
            .Include(o => o.StatusHistory)
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