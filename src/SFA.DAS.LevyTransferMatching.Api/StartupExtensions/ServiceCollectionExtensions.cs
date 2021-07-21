using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbConfiguration(this IServiceCollection services, string connectionString, IWebHostEnvironment hostingEnvironment)
        {
            if(hostingEnvironment.IsDevelopment())
            {
                services.AddSingleton<IManagedIdentityTokenProvider, LocalDbTokenProvider>();
            }
            else
            {
                services.AddSingleton<IManagedIdentityTokenProvider, ManagedIdentityTokenProvider>();
            }

            services.AddTransient<IConnectionFactory, SqlServerConnectionFactory>();

            services.AddTransient<IEmployerAccountRepository, EmployerAccountRepository>();
            services.AddTransient<IPledgeRepository, PledgeRepository>();
        }
    }
}
