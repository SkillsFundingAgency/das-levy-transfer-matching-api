using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;

public class CreditPledgeCommandHandler(IPledgeRepository repository) : IRequestHandler<CreditPledgeCommand>
{
    public async Task Handle(CreditPledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await repository.Get(request.PledgeId);

        pledge.Credit(request.Amount, UserInfo.System);

        await repository.Update(pledge);
    }
}