using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;

public class DeclineFundingCommandHandler(IApplicationRepository applicationRepository, ILogger<DeclineFundingCommandHandler> logger)
    : IRequestHandler<DeclineFundingCommand, DeclineFundingCommandResult>
{
    public async Task<DeclineFundingCommandResult> Handle(DeclineFundingCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, accountId: request.AccountId);

        if (application == null)
        {
            logger.LogInformation("The application for {Request} could not be found.", request);

            return new DeclineFundingCommandResult
            {
                Updated = false
            };
        }

        application.DeclineFunding(new UserInfo(request.UserId, request.UserDisplayName));
        await applicationRepository.Update(application);

        return new DeclineFundingCommandResult
        {
            Updated = true
        };
    }
}