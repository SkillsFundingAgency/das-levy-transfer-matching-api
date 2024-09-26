using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public interface IEmployerAccountRepository
{
    Task Add(EmployerAccount account);
    Task<EmployerAccount> Get(long accountId);
}

public class EmployerAccountRepository(LevyTransferMatchingDbContext dbContext) : IEmployerAccountRepository
{
    public async Task Add(EmployerAccount account)
    {
        await dbContext.AddAsync(account);
    }

    public async Task<EmployerAccount> Get(long accountId)
    {
        return await dbContext.EmployerAccounts.FindAsync(accountId);
    }
}