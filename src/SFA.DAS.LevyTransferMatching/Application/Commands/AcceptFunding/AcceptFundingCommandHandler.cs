using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding
{
    public class AcceptFundingCommandHandler : IRequestHandler<AcceptFundingCommand, AcceptFundingCommandResult>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IPledgeRepository _pledgeRepository;

        private readonly ILogger<AcceptFundingCommandHandler> _logger;

        public AcceptFundingCommandHandler(IApplicationRepository applicationRepository, IPledgeRepository pledgeRepository, ILogger<AcceptFundingCommandHandler> logger)
        {
            _applicationRepository = applicationRepository;
            _pledgeRepository = pledgeRepository;
            _logger = logger;
        }

        public async Task<AcceptFundingCommandResult> Handle(AcceptFundingCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.Get(request.ApplicationId, null, request.AccountId);
            
            if (application == null)
            {
                _logger.LogInformation($"The application for {request} could not be found.");

                return new AcceptFundingCommandResult
                {
                    Updated = false
                };
            }
            var pledge = await _pledgeRepository.Get(application.PledgeId);

            if (pledge == null)
            {
                _logger.LogInformation($"The pledge for {request} could not be found.");

                return new AcceptFundingCommandResult
                {
                    Updated = false
                };
            }

            application.AcceptFunding(new UserInfo(request.UserId, request.UserDisplayName), pledge);
            await _applicationRepository.Update(application);
            
            return new AcceptFundingCommandResult
            {
                Updated = true
            };
        }
    }
}