using System;
using System.IO;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.LevyTransferMatching.Api.StartupExtensions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Behaviours;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;

namespace SFA.DAS.LevyTransferMatching.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;

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

            Configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNLog();
            services.AddConfigurationOptions(Configuration);
            var config = Configuration.GetSection<LevyTransferMatchingApi>();

            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.RegisterValidatorsFromAssemblyContaining<CreatePledgeCommandValidator>();
                })
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddMediatR(typeof(CreatePledgeCommandHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            services.AddApplicationInsightsTelemetry(Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY"));
            services.AddDasHealthChecks(config);
            services.AddDbConfiguration(config.DatabaseConnectionString, _environment);
            services.AddNServiceBusClientUnitOfWork();
            services.AddCache(config, _environment)
                    .AddDasDataProtection(config, _environment)
                    .AddSwaggerGen()
                    .AddSwaggerGenNewtonsoftSupport();
        }

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

            app.AddSwagger();
        }

        public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
        {
            var config = Configuration.GetSection<LevyTransferMatchingApi>();
            serviceProvider.StartNServiceBus(config, _environment);
        }
    }
}
