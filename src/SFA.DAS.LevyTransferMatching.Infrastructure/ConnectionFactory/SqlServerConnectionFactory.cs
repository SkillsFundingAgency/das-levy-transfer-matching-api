using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

namespace SFA.DAS.LevyTransferMatching.Infrastructure
{
    public class SqlServerConnectionFactory : IConnectionFactory
    {
        private readonly LevyTransferMatchingApi _configuration;
        private readonly IManagedIdentityTokenProvider _managedIdentityTokenProvider;

        public SqlServerConnectionFactory(LevyTransferMatchingApi configuration, IManagedIdentityTokenProvider managedIdentityTokenProvider)
        {
            _configuration = configuration;
            _managedIdentityTokenProvider = managedIdentityTokenProvider;
        }

        public DbContextOptionsBuilder<TContext> AddConnection<TContext>(DbContextOptionsBuilder<TContext> builder, string connection) where TContext : DbContext
        {
            return builder.UseSqlServer(CreateConnection(connection));
        }

        public DbContextOptionsBuilder<TContext> AddConnection<TContext>(DbContextOptionsBuilder<TContext> builder, DbConnection connection) where TContext : DbContext
        {
            return builder.UseSqlServer(connection);
        }

        public DbConnection CreateConnection(string connection)
        {
            var sqlConnection = new SqlConnection(connection)
            {
                AccessToken = GetAccessToken(),
            };

            return sqlConnection;
        }

        private string GetAccessToken()
        {
            return _managedIdentityTokenProvider.GetSqlAccessTokenAsync()
                .GetAwaiter().GetResult();
        }
    }
}