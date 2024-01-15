using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public class EmployerAccountRepository : IEmployerAccountRepository
{
    private readonly LevyTransferMatchingDbContext _dbContext;

    public EmployerAccountRepository(LevyTransferMatchingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(EmployerAccount account)
    {
        await _dbContext.AddAsync(account);
    }

    public async Task<EmployerAccount> Get(long accountId)
    {
        return await _dbContext.EmployerAccounts.FindAsync(accountId);
    }
}