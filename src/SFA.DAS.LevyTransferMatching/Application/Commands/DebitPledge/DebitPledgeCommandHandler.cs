﻿using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;

public class DebitPledgeCommandHandler : IRequestHandler<DebitPledgeCommand, DebitPledgeCommandResult>
{
    private readonly IPledgeRepository _repository;
    private readonly ILogger<DebitPledgeCommandHandler> _logger;

    public DebitPledgeCommandHandler(IPledgeRepository repository, ILogger<DebitPledgeCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DebitPledgeCommandResult> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
    {
        var pledge = await _repository.Get(request.PledgeId);

        var success = pledge.Debit(request.Amount, request.ApplicationId, UserInfo.System);

        if (!success)
        {
            _logger.LogError("Debit of Pledge {PledgeId} in respect of Application {ApplicationId} failed", request.PledgeId, request.ApplicationId);
        }

        await _repository.Update(pledge);

        return new DebitPledgeCommandResult
        {
            IsSuccess = success
        };
    }
}