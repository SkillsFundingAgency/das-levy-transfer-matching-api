using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;

public class ApproveApplicationCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<ApproveApplicationCommand>
{
    public async Task Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, request.PledgeId, null);

        application.Approve(new UserInfo(request.UserId, request.UserDisplayName), request.AutomaticApproval);

        await applicationRepository.Update(application);
    }
}