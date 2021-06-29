using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LevyTransferMatching.Behaviours
{
    public class RetryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> _logger;

        public RetryBehaviour(ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogTrace("Invoked RetryBehaviour");

            return next();
        }
    }
}