using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

public class ManagedIdentityTokenProvider : IManagedIdentityTokenProvider
{
    public Task<string> GetSqlAccessTokenAsync()
    {
        var provider = new AzureServiceTokenProvider();
        return provider.GetAccessTokenAsync("https://database.windows.net/");
    }
}