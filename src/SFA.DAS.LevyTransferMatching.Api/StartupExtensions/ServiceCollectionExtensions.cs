using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.LevyTransferMatching.Abstractions.Audit;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Behaviours;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.EventHandlers;
using SFA.DAS.LevyTransferMatching.Infrastructure;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;
using SFA.DAS.LevyTransferMatching.Services;
using SFA.DAS.LevyTransferMatching.Services.Audit;
using SFA.DAS.LevyTransferMatching.Services.Events;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesForLevyTransferMatching(this IServiceCollection services, IWebHostEnvironment hostingEnvironment, LevyTransferMatchingApi config)
        {
            services.AddMediatR(typeof(CreateAccountCommand).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

            if (hostingEnvironment.IsDevelopment())
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
            services.AddTransient<IApplicationRepository, ApplicationRepository>();

            services.Scan(scan =>
                {
                    scan.FromAssembliesOf(typeof(EntityStateChangedHandler))
                        .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                })
                .AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            services.AddTransient<IDiffService, DiffService>();
            services.AddSingleton<ICostProjectionService, CostProjectionService>();
            services.AddSingleton<IMatchingCriteriaService, MatchingCriteriaService>();
            services.AddTransient<IDateTimeService, DateTimeService>(s => new DateTimeService(config.UtcNowOverride));
        }
    }
}
