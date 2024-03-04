using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

public class SqlServerConnectionFactory : IConnectionFactory
{
    private readonly IManagedIdentityTokenProvider _managedIdentityTokenProvider;

    public SqlServerConnectionFactory(IManagedIdentityTokenProvider managedIdentityTokenProvider)
    {
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