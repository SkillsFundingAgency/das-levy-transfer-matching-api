using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;

public class ExpireAcceptedFundingCommandHandler(IApplicationRepository applicationRepository, ILogger<ExpireAcceptedFundingCommandHandler> logger)
    : IRequestHandler<ExpireAcceptedFundingCommand, ExpireAcceptedFundingCommandResult>
{
    public async Task<ExpireAcceptedFundingCommandResult> Handle(ExpireAcceptedFundingCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId);

        if (application == null)
        {
            logger.LogInformation("The application for {Request} could not be found.", request);

            return new ExpireAcceptedFundingCommandResult
            {
                Updated = false
            };
        }

        application.ExpireAcceptedFunding(new UserInfo(request.UserId, request.UserDisplayName));
        await applicationRepository.Update(application);

        return new ExpireAcceptedFundingCommandResult
        {
            Updated = true
        };
    }
}