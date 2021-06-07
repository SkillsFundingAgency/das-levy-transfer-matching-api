using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public class LevyTransferMatchingDbContext : DbContext, ILevyTransferMatchingDbContext
    {
        public LevyTransferMatchingDbContext(DbContextOptions<LevyTransferMatchingDbContext> options) : base(options)
        {
        }

        public DbSet<PledgeDataModel> Pledges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
