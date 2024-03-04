using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<LevyTransferMatchingApi>(configuration.GetSection("LevyTransferMatchingApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApi>>().Value);
        services.Configure<FeatureToggles>(configuration.GetSection("FeatureToggles"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FeatureToggles>>().Value);
    }
}