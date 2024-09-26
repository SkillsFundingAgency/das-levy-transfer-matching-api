using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

public interface IConnectionFactory
{
    DbConnection CreateConnection(string connection);
}

public class SqlServerConnectionFactory(IManagedIdentityTokenProvider managedIdentityTokenProvider) : IConnectionFactory
{
    public DbConnection CreateConnection(string connection)
    {
        return new SqlConnection(connection)
        {
            AccessToken = GetAccessToken(),
        };
    }

    private string GetAccessToken()
    {
        return managedIdentityTokenProvider
            .GetSqlAccessTokenAsync()
            .GetAwaiter()
            .GetResult();
    }
}