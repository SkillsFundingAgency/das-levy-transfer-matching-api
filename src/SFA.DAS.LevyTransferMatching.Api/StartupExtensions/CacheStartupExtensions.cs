using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions;

public static class CacheStartupExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, LevyTransferMatchingApi config, IHostEnvironment environment)
    {
        if (string.IsNullOrEmpty(config.RedisConnectionString))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options => { options.Configuration = config.RedisConnectionString; });
        }

        return services;
    }
}