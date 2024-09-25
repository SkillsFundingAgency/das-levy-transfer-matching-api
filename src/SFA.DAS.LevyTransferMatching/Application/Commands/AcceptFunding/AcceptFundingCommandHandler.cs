using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;

public class AcceptFundingCommandHandler(IApplicationRepository applicationRepository, IPledgeRepository pledgeRepository, ILogger<AcceptFundingCommandHandler> logger)
    : IRequestHandler<AcceptFundingCommand, AcceptFundingCommandResult>
{
    public async Task<AcceptFundingCommandResult> Handle(AcceptFundingCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, null, request.AccountId);

        if (application == null)
        {
            logger.LogInformation("The application for {ApplicationId} could not be found.", request.ApplicationId);

            return new AcceptFundingCommandResult
            {
                Updated = false
            };
        }
        var pledge = await pledgeRepository.Get(application.PledgeId);

        if (pledge == null)
        {
            logger.LogInformation("The pledge for {ApplicationId} could not be found.", request.ApplicationId);

            return new AcceptFundingCommandResult
            {
                Updated = false
            };
        }

        var shouldRejectApplications = ShouldPendingApplicationsBeAutomaticallyClosed(pledge, request.ApplicationId);

        application.AcceptFunding(new UserInfo(request.UserId, request.UserDisplayName), shouldRejectApplications);
        await applicationRepository.Update(application);

        return new AcceptFundingCommandResult
        {
            Updated = true
        };
    }

    private static bool ShouldPendingApplicationsBeAutomaticallyClosed(Pledge pledge, int applicationId)
    {
        return pledge.Status == PledgeStatus.Closed && pledge.RemainingAmount <= 2000
           && pledge.Applications.Where(x => x.Id != applicationId).All(x => x.Status != ApplicationStatus.Approved);
    }
}
