using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Repositories
{
    public interface IPledgeRepository
    {
        Task Add(Pledge pledge);
        Task<Pledge> Get(int pledgeId);
    }
}