using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.LevyTransferMatching.Api.HttpResponseExtensions;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Api.StartupExtensions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;

namespace SFA.DAS.LevyTransferMatching.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<Startup> _logger;
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger<Startup> logger)
        {
            _environment = environment;
            _logger = logger;
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
            services.AddNLog().AddLogging();

            services.AddConfigurationOptions(Configuration);
            var config = Configuration.GetSection<LevyTransferMatchingApi>();

            _logger.LogInformation("Configuring Services");

            if (!_environment.IsDevelopment())
            {
                var azureAdConfiguration = Configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();

                if (azureAdConfiguration == null || string.IsNullOrEmpty(azureAdConfiguration.Identifier) || string.IsNullOrEmpty(azureAdConfiguration.Tenant))
                {
                    _logger.LogError("AzureAd config missing");
                }
                else
                {
                    _logger.LogInformation($"Identifier: {azureAdConfiguration.Identifier.Length}");
                    _logger.LogInformation($"Tenant: {azureAdConfiguration.Tenant.Length}");
                }

                var policies = new Dictionary<string, string>
                {
                    {PolicyNames.Default, RoleNames.Default}
                };

                services.AddAuthentication(azureAdConfiguration, policies);
            }

            services
                .AddMvc(o =>
                {
                    if (!_environment.IsDevelopment())
                    {
                        o.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                    }
                    o.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.RegisterValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
                })
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddApplicationInsightsTelemetry(Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY"));
            services.AddDasHealthChecks(config);
            services.AddServicesForLevyTransferMatching(_environment, config);

            services.AddEntityFrameworkForLevyTransferMatching(config)
                .AddEntityFrameworkUnitOfWork<LevyTransferMatchingDbContext>()
                .AddNServiceBusClientUnitOfWork();

            services.AddCache(config, _environment)
                .AddDasDataProtection(config, _environment)
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo {Title = "LevyTransferMatchingApi", Version = "v1"});
                    c.OperationFilter<SwaggerVersionHeaderFilter>();
                })
                .AddSwaggerGenNewtonsoftSupport();

            services.AddApiVersioning(opt => {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });
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

            app.UseExceptionHandler(c => { c.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    if (exception is ValidationException validationException)
                    {
                        var errorResponse = new FluentValidationErrorResponse
                        {
                            Errors = validationException.Errors
                        };

                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteJsonAsync(errorResponse);
                    }
                });
            });

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