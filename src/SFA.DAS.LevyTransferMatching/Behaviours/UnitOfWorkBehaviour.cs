using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.Behaviours
{
    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> _logger;
        private readonly LevyTransferMatchingDbContext _dbContext;

        public UnitOfWorkBehaviour(LevyTransferMatchingDbContext dbContext, ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogTrace("Invoked UnitOfWorkBehaviour");

            var result = await next();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
