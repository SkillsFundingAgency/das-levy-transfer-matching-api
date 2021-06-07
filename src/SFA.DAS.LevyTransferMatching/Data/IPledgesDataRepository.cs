using SFA.DAS.LevyTransferMatching.Models;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public interface IPledgesDataRepository
    {
        Task Add(Pledge pledge);
    }
}