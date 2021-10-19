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
        private readonly ILogger<AcceptFundingCommandHandler> _logger;

        public AcceptFundingCommandHandler(IApplicationRepository applicationRepository, ILogger<AcceptFundingCommandHandler> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        public async Task<AcceptFundingCommandResult> Handle(AcceptFundingCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.Get(null, request.ApplicationId, request.AccountId);

            if (application == null)
            {
                _logger.LogInformation($"The application for {request} could not be found.");

                return new AcceptFundingCommandResult
                {
                    Updated = false
                };
            }


            try
            {
                application.AcceptFunding(new UserInfo(request.UserId, request.UserDisplayName));

            }
            catch (System.Exception exception)
            {
                _logger.LogInformation($"An error occurred in the accept funding method call :\n\n{exception}");
            }

            try
            {
                await _applicationRepository.Update(application);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"An error occurred in the application repository method call :\n\n{e}");
            }
            

            return new AcceptFundingCommandResult
            {
                Updated = true
            };
        }
    }
}