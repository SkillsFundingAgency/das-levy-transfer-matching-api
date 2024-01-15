using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;

public class UndoApplicationApprovalCommandHandler : IRequestHandler<UndoApplicationApprovalCommand>
{
    private readonly IApplicationRepository _applicationRepository;

    public UndoApplicationApprovalCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task Handle(UndoApplicationApprovalCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.Get(request.ApplicationId, request.PledgeId);
        application.UndoApproval(UserInfo.System);
        await _applicationRepository.Update(application);
    }
}