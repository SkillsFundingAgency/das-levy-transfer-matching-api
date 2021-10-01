using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories
{
    public interface IApplicationRepository
    {
        Task Add(Models.Application application);
        Task Update(Models.Application application);
        Task<Models.Application> Get(int? pledgeId, int applicationId, long? accountId);
    }
}