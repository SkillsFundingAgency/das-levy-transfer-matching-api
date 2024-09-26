using Microsoft.Extensions.Logging;
using SFA.DAS.UnitOfWork.Managers;

namespace SFA.DAS.LevyTransferMatching.Behaviours;

public class UnitOfWorkBehaviour<TRequest, TResponse>(ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger, IUnitOfWorkManager unitOfWorkManager)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogTrace("Invoked UnitOfWorkBehaviour");

        TResponse result;

        try
        {
            await unitOfWorkManager.BeginAsync();

            result = await next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception handled by UnitOfWorkBehaviour");

            await unitOfWorkManager.EndAsync(ex);
            
            throw;
        }

        await unitOfWorkManager.EndAsync();

        return result;
    }
}