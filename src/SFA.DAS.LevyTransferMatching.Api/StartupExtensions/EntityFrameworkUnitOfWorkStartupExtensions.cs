using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class EntityFrameworkUnitOfWorkStartupExtensions
    {
        public static IServiceCollection AddEntityFrameworkForLevyTransferMatching(this IServiceCollection services, LevyTransferMatchingApi config)
        {
            return services.AddScoped(p =>
            {
                var unitOfWorkContext = p.GetService<IUnitOfWorkContext>();
                LevyTransferMatchingDbContext dbContext;
                try
                {
                    var synchronizedStorageSession = unitOfWorkContext.Get<SynchronizedStorageSession>();
                    var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
                    var optionsBuilder = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>().UseSqlServer(sqlStorageSession.Connection);
                    //optionsBuilder.UseLoggerFactory(DebugLoggingFactory);
                    dbContext = new LevyTransferMatchingDbContext(optionsBuilder.Options);
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
        
        public static readonly LoggerFactory DebugLoggingFactory =
            new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });
    }
}
