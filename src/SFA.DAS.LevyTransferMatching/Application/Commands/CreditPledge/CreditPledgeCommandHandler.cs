﻿using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;

public class CreditPledgeCommandHandler : IRequestHandler<CreditPledgeCommand>
{
    private readonly IPledgeRepository _repository;

    public CreditPledgeCommandHandler(IPledgeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreditPledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await _repository.Get(request.PledgeId);

        pledge.Credit(request.Amount, UserInfo.System);

        await _repository.Update(pledge);
    }
}