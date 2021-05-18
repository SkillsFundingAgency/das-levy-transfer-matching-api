using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.LevyTransferMatching.Api.HealthChecks;
using SFA.DAS.LevyTransferMatching.Api.HttpResponseExtensions;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class HealthCheckStartupExtensions
    {
        public static IServiceCollection AddDasHealthChecks(this IServiceCollection services, LevyTransferMatchingApi config)
        {
            services
                .AddHealthChecks()
                .AddDbContextCheck<LevyTransferMatchingDbContext>("Sql Health Check")
                .AddCheck<NServiceBusHealthCheck>("NService Bus health check")
                .AddRedis(config.RedisConnectionString, "Redis" )
            ;

            return services;
        }

        public static IApplicationBuilder UseDasHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/ping", new HealthCheckOptions
            {
                Predicate = (_) => false,
                ResponseWriter = (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("");
                }
            });

            return app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = (c, r) => c.Response.WriteJsonAsync(new
                {
                    r.Status,
                    r.TotalDuration,
                    Results = r.Entries.ToDictionary(
                        e => e.Key,
                        e => new
                        {
                            e.Value.Status,
                            e.Value.Duration,
                            e.Value.Description,
                            e.Value.Data
                        })
                })
            });
        }
    }
}
