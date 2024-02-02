using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;

public class RejectApplicationCommandHandler : IRequestHandler<RejectApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;

    public RejectApplicationCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task Handle(RejectApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.Get(request.ApplicationId, request.PledgeId, null);

        application.Reject(new UserInfo(request.UserId, request.UserDisplayName));

        await _applicationRepository.Update(application);
    }
}