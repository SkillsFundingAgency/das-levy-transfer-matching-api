using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;

public class DebitApplicationCommandHandler(IApplicationRepository repository) : IRequestHandler<DebitApplicationCommand>
{
    public async Task Handle(DebitApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await repository.Get(request.ApplicationId);
        application.Debit(request.NumberOfApprentices, request.Amount, request.MaxAmount, UserInfo.System);

        await repository.Update(application);
    }
}