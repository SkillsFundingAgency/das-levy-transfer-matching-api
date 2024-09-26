using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions;

public static class EntityFrameworkUnitOfWorkStartupExtensions
{
    public static IServiceCollection AddEntityFrameworkForLevyTransferMatching(this IServiceCollection services, LevyTransferMatchingApi config)
    {
        return services.AddScoped(provider =>
        {
            var unitOfWorkContext = provider.GetService<IUnitOfWorkContext>();
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            LevyTransferMatchingDbContext dbContext;
            
            try
            {
                var synchronizedStorageSession = unitOfWorkContext.Get<SynchronizedStorageSession>();
                var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
                var optionsBuilder = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>().UseSqlServer(sqlStorageSession.Connection);
                dbContext = new LevyTransferMatchingDbContext(sqlStorageSession.Connection, config, azureServiceTokenProvider, optionsBuilder.Options);
                dbContext.Database.UseTransaction(sqlStorageSession.Transaction);
            }
            catch (KeyNotFoundException)
            {
                var optionsBuilder = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>().UseSqlServer(config.DatabaseConnectionString);
                optionsBuilder.UseLoggerFactory(DebugLoggingFactory);
                dbContext = new LevyTransferMatchingDbContext(optionsBuilder.Options);
            }

            return dbContext;
        });
    }

    private static readonly LoggerFactory DebugLoggingFactory = new(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
}