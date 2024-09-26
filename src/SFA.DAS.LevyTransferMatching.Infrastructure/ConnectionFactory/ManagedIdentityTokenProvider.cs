using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

public interface IManagedIdentityTokenProvider
{
    Task<string> GetSqlAccessTokenAsync();
}

public class ManagedIdentityTokenProvider : IManagedIdentityTokenProvider
{
    // Take advantage of the built-in caching AzureServiceTokenProvider provides 
    private readonly AzureServiceTokenProvider _azureServiceTokenProvider = new();
    
    public Task<string> GetSqlAccessTokenAsync()
    {
        return _azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/");
    }
}