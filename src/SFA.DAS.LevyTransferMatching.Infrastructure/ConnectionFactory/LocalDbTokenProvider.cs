using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory
{
    public class LocalDbTokenProvider : IManagedIdentityTokenProvider
    {
        public Task<string> GetSqlAccessTokenAsync()
        {
            return Task.FromResult(default(string));
        }
    }
}