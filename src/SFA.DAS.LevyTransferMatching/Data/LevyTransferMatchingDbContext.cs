using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data;

public class LevyTransferMatchingDbContext : DbContext
{
    public DbSet<Pledge> Pledges { get; set; }
    public DbSet<EmployerAccount> EmployerAccounts { get; set; }
    public DbSet<Models.Application> Applications { get; set; }
    public DbSet<Audit> Audits { get; set; }
    
    public LevyTransferMatchingDbContext(DbContextOptions<LevyTransferMatchingDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LevyTransferMatchingDbContext).Assembly);
    }
}