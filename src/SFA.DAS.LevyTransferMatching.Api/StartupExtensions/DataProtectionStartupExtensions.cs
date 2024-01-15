using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions;

public static class DataProtectionStartupExtensions
{
    public static IServiceCollection AddDasDataProtection(this IServiceCollection services, LevyTransferMatchingApi configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return services;
        }
        
        var redisConnectionString = configuration.RedisConnectionString;
        var dataProtectionKeysDatabase = configuration.DataProtectionKeysDatabase;

        var redis = ConnectionMultiplexer.Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

        services.AddDataProtection()
            .SetApplicationName("das-levy-transfer-matching-api")
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

        return services;
    }
}