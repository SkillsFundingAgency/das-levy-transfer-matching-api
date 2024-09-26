using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;

public class UndoApplicationApprovalCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<UndoApplicationApprovalCommand>
{
    public async Task Handle(UndoApplicationApprovalCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, request.PledgeId);
        application.UndoApproval(UserInfo.System);
        await applicationRepository.Update(application);
    }
}