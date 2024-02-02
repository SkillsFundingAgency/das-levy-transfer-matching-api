using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using NServiceBus.ObjectBuilder.MSDependencyInjection;  
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.LevyTransferMatching.Api.HttpResponseExtensions;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Api.StartupExtensions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.NServiceBus.Features.ClientOutbox.Data;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;

namespace SFA.DAS.LevyTransferMatching.Api;

public class Startup
{
    private readonly IHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;
        _configuration = configuration.BuildDasConfiguration();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddNLog();
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });
        
        services.AddConfigurationOptions(_configuration);
        var config = _configuration.GetSection<LevyTransferMatchingApi>();

        if (!_environment.IsDevelopment())
        {
            var azureAdConfiguration = _configuration
                .GetSection("AzureAd")
                .Get<AzureActiveDirectoryConfiguration>();

            var policies = new Dictionary<string, string>
            {
                { PolicyNames.Default, RoleNames.Default }
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }

        services.AddMvc(mvcOptions =>
            {
                if (!_environment.IsDevelopment())
                {
                    mvcOptions.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                }

                mvcOptions.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            })
            .AddNewtonsoftJson();

        services
            .AddControllers()
            .AddNewtonsoftJson(x => { x.SerializerSettings.Converters.Add(new StringEnumConverter()); });

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<Startup>()
            .AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();
        

        services.AddApplicationInsightsTelemetry();
        services.AddDasHealthChecks(config);
        services.AddServicesForLevyTransferMatching(_environment, config);

        services.AddEntityFrameworkForLevyTransferMatching(config)
            .AddEntityFrameworkUnitOfWork<LevyTransferMatchingDbContext>()
            .AddNServiceBusClientUnitOfWork();

        services.AddCache(config, _environment)
            .AddDasDataProtection(config, _environment)
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "LevyTransferMatchingApi", Version = "v1" });
                options.OperationFilter<SwaggerVersionHeaderFilter>();
            })
            .AddSwaggerGenNewtonsoftSupport();

        services.AddApiVersioning(opt => { opt.ApiVersionReader = new HeaderApiVersionReader("X-Version"); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseDasHealthChecks();

        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
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

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "LevyTransferMatching v1");
            options.RoutePrefix = string.Empty;
        });
    }

    public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
    {
        var config = _configuration.GetSection<LevyTransferMatchingApi>();
        serviceProvider.StartNServiceBus(config);
        
        // Replacing ClientOutboxPersisterV2 with a local version to fix unit of work issue due to propagating Task up the chain rather than awaiting on DB Command.
        // not clear why this fixes the issue. Attempted to make the change in SFA.DAS.Nservicebus.SqlServer however it conflicts when upgraded with SFA.DAS.UnitOfWork.Nservicebus
        // which would require upgrading to NET6 to resolve.
        var serviceDescriptor = serviceProvider.FirstOrDefault(serv => serv.ServiceType == typeof(IClientOutboxStorageV2));
        serviceProvider.Remove(serviceDescriptor);
        serviceProvider.AddScoped<IClientOutboxStorageV2, ClientOutboxPersisterV2>();
    }
}