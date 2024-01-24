using System.Data;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Data;

public class LevyTransferMatchingDbContext : DbContext
{
    private readonly IDbConnection _connection;
    private readonly LevyTransferMatchingApi _configuration;
    private readonly AzureServiceTokenProvider _azureServiceTokenProvider;
    public DbSet<Pledge> Pledges { get; set; }
    public DbSet<EmployerAccount> EmployerAccounts { get; set; }
    public DbSet<Models.Application> Applications { get; set; }
    public DbSet<Audit> Audits { get; set; }
    
    public LevyTransferMatchingDbContext(DbContextOptions<LevyTransferMatchingDbContext> options) : base(options)
    {
    }

    public LevyTransferMatchingDbContext(IDbConnection connection, LevyTransferMatchingApi configuration, AzureServiceTokenProvider azureServiceTokenProvider, DbContextOptions options) : base(options)
    {
        _connection = connection;
        _configuration = configuration;
        _azureServiceTokenProvider = azureServiceTokenProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_configuration is null || _azureServiceTokenProvider is null)
        {
            return;
        }
        
        optionsBuilder.UseSqlServer(_connection as SqlConnection);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pledge>().ToTable("Pledge");
        modelBuilder.Entity<Pledge>().HasKey(x => x.Id);
        modelBuilder.Entity<Pledge>().HasOne(x => x.EmployerAccount).WithMany();
        modelBuilder.Entity<Pledge>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Pledge>().HasMany(x => x.Locations).WithOne();
        modelBuilder.Entity<Pledge>().Metadata.FindNavigation(nameof(Pledge.Locations))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        modelBuilder.Entity<Pledge>().HasMany(x => x.Applications).WithOne();
        modelBuilder.Entity<Pledge>().Metadata.FindNavigation(nameof(Pledge.Applications))
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<PledgeLocation>().ToTable("PledgeLocation");

        modelBuilder.Entity<Models.Application>().ToTable("Application");
        modelBuilder.Entity<Models.Application>().HasKey(x => x.Id);
        modelBuilder.Entity<Models.Application>().HasOne(x => x.EmployerAccount).WithMany();
        modelBuilder.Entity<Models.Application>().HasOne(x => x.Pledge).WithMany(x => x.Applications);
        modelBuilder.Entity<Models.Application>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Models.Application>().HasMany(x => x.EmailAddresses).WithOne();
        modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.EmailAddresses)).SetPropertyAccessMode(PropertyAccessMode.Field);
        modelBuilder.Entity<Models.Application>().HasMany(x => x.StatusHistory).WithOne();
        modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.StatusHistory)).SetPropertyAccessMode(PropertyAccessMode.Field);
        modelBuilder.Entity<Models.Application>().HasMany(x => x.ApplicationLocations).WithOne();
        modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.ApplicationLocations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        modelBuilder.Entity<Models.Application>().HasMany(x => x.ApplicationCostProjections).WithOne().IsRequired(true);
        modelBuilder.Entity<Models.Application>().Metadata.FindNavigation(nameof(Models.Application.ApplicationCostProjections)).SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<ApplicationEmailAddress>().ToTable("ApplicationEmailAddress");
        modelBuilder.Entity<ApplicationEmailAddress>().HasKey(x => x.Id);

        modelBuilder.Entity<ApplicationLocation>().ToTable("ApplicationLocation");
        modelBuilder.Entity<ApplicationLocation>().HasKey(x => x.Id);

        modelBuilder.Entity<ApplicationStatusHistory>().ToTable("ApplicationStatusHistory");
        modelBuilder.Entity<ApplicationStatusHistory>().HasKey(x => x.Id);

        modelBuilder.Entity<EmployerAccount>().ToTable("EmployerAccount");
        modelBuilder.Entity<EmployerAccount>().HasKey(x => x.Id);

        modelBuilder.Entity<Audit>().ToTable("Audit");
        modelBuilder.Entity<Audit>().HasKey(x => x.Id);
    }
    
    public override void Dispose()
    {
        _connection?.Dispose();
        
        base.Dispose();
    }
}