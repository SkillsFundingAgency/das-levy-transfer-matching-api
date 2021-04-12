using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.LevyTransferMatching.Api.StartupExtensions;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;

namespace SFA.DAS.LevyTransferMatching.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;

            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
#endif
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }

            _configuration = config.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationOptions(_configuration);
            var config = _configuration.GetSection<LevyTransferMatchingApi>();

            services.AddControllers();
            services.AddDasHealthChecks(config);
            services.AddDbConfiguration(config.DatabaseConnectionString, _environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseDasHealthChecks();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
