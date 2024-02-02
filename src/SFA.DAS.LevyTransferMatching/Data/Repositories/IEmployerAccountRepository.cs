using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories;

public interface IEmployerAccountRepository
{
    Task Add(EmployerAccount account);
    Task<EmployerAccount> Get(long accountId);
}