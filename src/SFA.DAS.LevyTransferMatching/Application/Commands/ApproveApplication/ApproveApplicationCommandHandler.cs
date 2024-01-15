using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;

public class ApproveApplicationCommandHandler : IRequestHandler<ApproveApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;

    public ApproveApplicationCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.Get(request.ApplicationId, request.PledgeId, null);

        application.Approve(new UserInfo(request.UserId, request.UserDisplayName), request.AutomaticApproval);

        await _applicationRepository.Update(application);
    }
}