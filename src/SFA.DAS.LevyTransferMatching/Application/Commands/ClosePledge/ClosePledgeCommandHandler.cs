using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;

public class ClosePledgeCommandHandler : IRequestHandler<ClosePledgeCommand>
{
    private readonly IPledgeRepository _pledgeRepository;

    public ClosePledgeCommandHandler(IPledgeRepository pledgeRepository)
    {
        _pledgeRepository = pledgeRepository;
    }

    public async Task Handle(ClosePledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await _pledgeRepository.Get(request.PledgeId);

        if (pledge == null)
        {
            throw new AggregateNotFoundException<Pledge>(request.PledgeId);
        }

        pledge.ClosePledge(new UserInfo(request.UserId, request.UserDisplayName));

        await _pledgeRepository.Update(pledge);
    }
}