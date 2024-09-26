using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SFA.DAS.LevyTransferMatching.Behaviours;

public class RetryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly AsyncRetryPolicy _concurrencyFailureRetryPolicy;

    public RetryBehaviour(ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger)
    {
        _concurrencyFailureRetryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(3,
                _ => TimeSpan.FromMilliseconds(100),
                (exception, _) => logger.LogInformation(exception, "Retrying following DbUpdateConcurrencyException"));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await _concurrencyFailureRetryPolicy.ExecuteAsync(async () => await next());
    }
}