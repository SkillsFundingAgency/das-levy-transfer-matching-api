using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;

public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand>
{
    private readonly IApplicationRepository _applicationRepository;

    public WithdrawApplicationCommandHandler(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.Get(request.ApplicationId, null, request.AccountId);

        application.Withdraw(new UserInfo(request.UserId, request.UserDisplayName));

        await _applicationRepository.Update(application);
    }
}