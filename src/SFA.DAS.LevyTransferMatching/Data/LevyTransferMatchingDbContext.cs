using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public class LevyTransferMatchingDbContext : DbContext, ILevyTransferMatchingDbContext
    {
        public LevyTransferMatchingDbContext(DbContextOptions<LevyTransferMatchingDbContext> options) : base(options)
        {
        }

        public DbSet<Pledge> Pledges { get; set; }
        public DbSet<EmployerAccount> EmployerAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pledge>().ToTable("Pledge");
            modelBuilder.Entity<Pledge>().HasKey(x => x.Id);
            modelBuilder.Entity<Pledge>().HasOne(x => x.EmployerAccount).WithMany();
            modelBuilder.Entity<Pledge>().Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
