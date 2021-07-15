using System.Data.Common;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbConfiguration(this IServiceCollection services, string connectionString, IWebHostEnvironment hostingEnvironment)
        {
            services.AddTransient<DbConnection>(provider => new SqlConnection(connectionString));
            if (hostingEnvironment.IsDevelopment())
            {
                services.AddTransient<IDbContextFactory<LevyTransferMatchingDbContext>>(provider => new DbContextFactory(new SqlConnection(connectionString), provider.GetService<ILoggerFactory>(), null));
            }
            else
            {
                services.AddTransient<IDbContextFactory<LevyTransferMatchingDbContext>>(provider => new DbContextFactory(new SqlConnection(connectionString), provider.GetService<ILoggerFactory>(), new AzureServiceTokenProvider()));
            }
            services.AddScoped<LevyTransferMatchingDbContext>(provider => provider.GetService<IDbContextFactory<LevyTransferMatchingDbContext>>().CreateDbContext());
            services.AddScoped<ILevyTransferMatchingDbContext>(provider => provider.GetService<LevyTransferMatchingDbContext>());

            services.AddTransient<IEmployerAccountRepository, EmployerAccountRepository>();
        }
    }
}
