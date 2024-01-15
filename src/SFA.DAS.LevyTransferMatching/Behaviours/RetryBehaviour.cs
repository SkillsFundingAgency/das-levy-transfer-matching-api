using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SFA.DAS.LevyTransferMatching.Behaviours;

public class RetryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> _logger;
    public readonly AsyncRetryPolicy ConcurrencyFailureRetryPolicy;

    public RetryBehaviour(ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;

        ConcurrencyFailureRetryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromMilliseconds(100),
                (exception, span) => _logger.LogInformation(exception, "Retrying following DbUpdateConcurrencyException"));
    }
        
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await ConcurrencyFailureRetryPolicy.ExecuteAsync(async () => await next());
    }
}