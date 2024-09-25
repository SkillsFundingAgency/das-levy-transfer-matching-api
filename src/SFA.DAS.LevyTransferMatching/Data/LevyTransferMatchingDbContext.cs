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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LevyTransferMatchingDbContext).Assembly);
    }
}