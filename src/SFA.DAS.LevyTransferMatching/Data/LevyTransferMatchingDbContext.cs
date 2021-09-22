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
        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pledge>().ToTable("Pledge");
            modelBuilder.Entity<Pledge>().HasKey(x => x.Id);
            modelBuilder.Entity<Pledge>().HasOne(x => x.EmployerAccount).WithMany();
            modelBuilder.Entity<Pledge>().Property(x => x.RowVersion).IsRowVersion();
            modelBuilder.Entity<Pledge>().HasMany(x => x.Locations).WithOne();
            modelBuilder.Entity<Pledge>().Metadata.FindNavigation(nameof(Pledge.Locations))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<PledgeLocation>().ToTable("PledgeLocation");

            modelBuilder.Entity<Models.Application>().ToTable("Application");
            modelBuilder.Entity<Models.Application>().HasKey(x => x.Id);
            modelBuilder.Entity<Models.Application>().HasOne(x => x.EmployerAccount).WithMany();
            modelBuilder.Entity<Models.Application>().HasOne(x => x.Pledge).WithMany(x => x.Applications);
            modelBuilder.Entity<Models.Application>().Property(x => x.RowVersion).IsRowVersion();
            modelBuilder.Entity<Models.Application>().HasMany(x => x.EmailAddresses).WithOne();
            modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.EmailAddresses)).SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.StatusHistory)).SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<ApplicationEmailAddress>().ToTable("ApplicationEmailAddress");
            modelBuilder.Entity<ApplicationEmailAddress>().HasKey(x => x.Id);

            modelBuilder.Entity<ApplicationStatusHistory>().ToTable("ApplicationStatusHistory");
            modelBuilder.Entity<ApplicationStatusHistory>().HasKey(x => x.Id);

            modelBuilder.Entity<EmployerAccount>().ToTable("EmployerAccount");
            modelBuilder.Entity<EmployerAccount>().HasKey(x => x.Id);

            modelBuilder.Entity<Audit>().ToTable("Audit");
            modelBuilder.Entity<Audit>().HasKey(x => x.Id);
        }
    }
}
