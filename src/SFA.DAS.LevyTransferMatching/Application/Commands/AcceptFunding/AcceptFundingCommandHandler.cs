using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding
{
    public class AcceptFundingCommandHandler : IRequestHandler<AcceptFundingCommand, AcceptFundingCommandResult>
    {
        private readonly IApplicationRepository _applicationRepository;

        public AcceptFundingCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<AcceptFundingCommandResult> Handle(AcceptFundingCommand request, CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.Get(null, request.ApplicationId, request.AccountId);

            if (application == null)
            {
                return new AcceptFundingCommandResult
                {
                    Updated = false
                };
            }

            application.AcceptFunding(new UserInfo(request.UserId, request.UserDisplayName));

            await _applicationRepository.Update(application);

            return new AcceptFundingCommandResult
            {
                Updated = true
            };
        }
    }
}