using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
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

            var shouldRejectApplications = ShouldPendingApplicationsBeAutomaticallyClosed(pledge);

            application.AcceptFunding(new UserInfo(request.UserId, request.UserDisplayName), shouldRejectApplications);
            await _applicationRepository.Update(application);
            
            return new AcceptFundingCommandResult
            {
                Updated = true
            };
        }

        private bool ShouldPendingApplicationsBeAutomaticallyClosed(Pledge pledge)
        {
            return pledge.Status == PledgeStatus.Closed && pledge.RemainingAmount <= 2000
                && !pledge.Applications.Any(x => x.Status == ApplicationStatus.Approved);
        }
    }
}