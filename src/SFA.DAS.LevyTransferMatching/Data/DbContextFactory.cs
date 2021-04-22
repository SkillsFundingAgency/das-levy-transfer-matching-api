using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LevyTransferMatching.Data
{
    public class DbContextFactory: IDbContextFactory<LevyTransferMatchingDbContext>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly ILoggerFactory _loggerFactory;
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;

        public DbContextFactory(SqlConnection sqlConnection, ILoggerFactory loggerFactory, AzureServiceTokenProvider azureServiceTokenProvider)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
            _sqlConnection = sqlConnection;
            if (_azureServiceTokenProvider != null)
            {
                _sqlConnection.AccessToken = _azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/").GetAwaiter().GetResult();
            }
            _loggerFactory = loggerFactory;
        }

        public LevyTransferMatchingDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseSqlServer(_sqlConnection)
                .UseLoggerFactory(_loggerFactory);

            var dbContext = new LevyTransferMatchingDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}
