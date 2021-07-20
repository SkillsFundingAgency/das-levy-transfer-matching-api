using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public interface ILevyTransferMatchingDbContext
    {
        DbSet<Pledge> Pledges { get; set; }
        DbSet<EmployerAccount> EmployerAccounts { get; set; }
        DbSet<PledgeLocation> PledgeLocations { get; set; }
    }
}