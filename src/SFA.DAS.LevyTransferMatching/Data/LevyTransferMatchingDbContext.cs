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
        public DbSet<Models.Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pledge>().ToTable("Pledge");
            modelBuilder.Entity<Pledge>().HasKey(x => x.Id);
            modelBuilder.Entity<Pledge>().HasOne(x => x.EmployerAccount).WithMany();
            modelBuilder.Entity<Pledge>().Property(x => x.RowVersion).IsRowVersion();

            modelBuilder.Entity<PledgeLocation>().ToTable("PledgeLocation");

            modelBuilder.Entity<Models.Application>().ToTable("Application");
            modelBuilder.Entity<Models.Application>().HasKey(x => x.Id);
            modelBuilder.Entity<Models.Application>().HasOne(x => x.EmployerAccount).WithMany();
            modelBuilder.Entity<Models.Application>().HasOne(x => x.Pledge).WithMany();
            modelBuilder.Entity<Models.Application>().Property(x => x.RowVersion).IsRowVersion();
            modelBuilder.Entity<Models.Application>().HasMany(x => x.EmailAddresses).WithOne();
            modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.EmailAddresses))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Models.ApplicationEmailAddress>().ToTable("ApplicationEmailAddress");
            modelBuilder.Entity<Models.ApplicationEmailAddress>().HasKey(x => x.Id);
        }
    }
}
