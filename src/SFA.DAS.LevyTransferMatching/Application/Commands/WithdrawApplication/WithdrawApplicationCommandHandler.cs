using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;

public class WithdrawApplicationCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<WithdrawApplicationCommand>
{
    public async Task Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.Get(request.ApplicationId, null, request.AccountId);

        application.Withdraw(new UserInfo(request.UserId, request.UserDisplayName));

        await applicationRepository.Update(application);
    }
}