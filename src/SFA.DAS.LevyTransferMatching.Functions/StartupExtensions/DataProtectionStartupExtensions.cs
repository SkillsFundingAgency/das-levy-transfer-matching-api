using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.LevyTransferMatching.Functions.StartupExtensions
{
    public static class DataProtectionStartupExtensions
    {
        public static IServiceCollection AddDasDataProtection(this IServiceCollection services, LevyTransferMatchingFunctions configuration)
        {
            if (string.IsNullOrEmpty(configuration.RedisConnectionString) ||
                string.IsNullOrEmpty(configuration.DataProtectionKeysDatabase))
            {
                return services;
            }
            
            services.AddDataProtection()
                .SetApplicationName("das-levy-transfer-matching-functions")
                .PersistKeysToStackExchangeRedis(CreateConnectionMultiplexer(configuration), "DataProtection-Keys");

            return services;
        }

        private static ConnectionMultiplexer CreateConnectionMultiplexer(LevyTransferMatchingFunctions configuration) => ConnectionMultiplexer.Connect($"{configuration.RedisConnectionString},{configuration.DataProtectionKeysDatabase}");
    }
}
