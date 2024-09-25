using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;

public class DebitPledgeCommandHandler(IPledgeRepository repository, ILogger<DebitPledgeCommandHandler> logger)
    : IRequestHandler<DebitPledgeCommand, DebitPledgeCommandResult>
{
    public async Task<DebitPledgeCommandResult> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await repository.Get(request.PledgeId);

        var success = pledge.Debit(request.Amount, request.ApplicationId, UserInfo.System);

        if (!success)
        {
            logger.LogError("Debit of Pledge {PledgeId} in respect of Application {ApplicationId} failed", request.PledgeId, request.ApplicationId);
        }

        await repository.Update(pledge);

        return new DebitPledgeCommandResult
        {
            IsSuccess = success
        };
    }
}