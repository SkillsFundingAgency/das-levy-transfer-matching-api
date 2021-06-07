using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public interface ILevyTransferMatchingDbContext
    {
        DbSet<PledgeDataModel> Pledges { get; set; }
    }
}