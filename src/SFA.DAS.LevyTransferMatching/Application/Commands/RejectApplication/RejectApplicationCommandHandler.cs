using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;

public class RejectApplicationCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<RejectApplicationCommand>
{
    public async Task Handle(RejectApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, request.PledgeId, null);

        application.Reject(new UserInfo(request.UserId, request.UserDisplayName));

        await applicationRepository.Update(application);
    }
}