using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

public interface IManagedIdentityTokenProvider
{
    Task<string> GetSqlAccessTokenAsync();
}