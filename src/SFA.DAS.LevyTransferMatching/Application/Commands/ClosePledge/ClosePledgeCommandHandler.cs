using SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;

public class ClosePledgeCommandHandler(IPledgeRepository pledgeRepository) : IRequestHandler<ClosePledgeCommand>
{
    public async Task Handle(ClosePledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await pledgeRepository.Get(request.PledgeId);

        if (pledge == null)
        {
            throw new AggregateNotFoundException<Pledge>(request.PledgeId);
        }

        pledge.ClosePledge(new UserInfo(request.UserId, request.UserDisplayName));

        await pledgeRepository.Update(pledge);
    }
}