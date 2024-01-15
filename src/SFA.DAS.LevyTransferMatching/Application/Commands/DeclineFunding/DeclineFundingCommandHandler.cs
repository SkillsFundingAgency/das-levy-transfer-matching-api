using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;

public class DeclineFundingCommandHandler : IRequestHandler<DeclineFundingCommand, DeclineFundingCommandResult>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILogger<DeclineFundingCommandHandler> _logger;

    public DeclineFundingCommandHandler(IApplicationRepository applicationRepository, ILogger<DeclineFundingCommandHandler> logger)
    {
        _applicationRepository = applicationRepository;
        _logger = logger;
    }

    public async Task<DeclineFundingCommandResult> Handle(DeclineFundingCommand request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.Get(request.ApplicationId, accountId: request.AccountId);

        if (application == null)
        {
            _logger.LogInformation($"The application for {request} could not be found.");

            return new DeclineFundingCommandResult
            {
                Updated = false
            };
        }

        application.DeclineFunding(new UserInfo(request.UserId, request.UserDisplayName));
        await _applicationRepository.Update(application);

        return new DeclineFundingCommandResult
        {
            Updated = true
        };
    }
}