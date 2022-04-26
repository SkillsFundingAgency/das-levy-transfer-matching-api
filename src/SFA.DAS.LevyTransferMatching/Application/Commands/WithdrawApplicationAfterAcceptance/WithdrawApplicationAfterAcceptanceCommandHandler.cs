using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommandHandler : IRequestHandler<WithdrawApplicationAfterAcceptanceCommand>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IPledgeRepository _pledgeRepository;

        public WithdrawApplicationAfterAcceptanceCommandHandler(IApplicationRepository applicationRepository, IPledgeRepository pledgeRepository)
        {
            _applicationRepository = applicationRepository;
            _pledgeRepository = pledgeRepository;
        }

        public async Task<Unit> Handle(WithdrawApplicationAfterAcceptanceCommand request, CancellationToken cancellationToken)
        {
            var userInfo = new UserInfo(request.UserId, request.UserDisplayName);

            var application = await _applicationRepository.Get(request.ApplicationId);
            var pledge = await _pledgeRepository.Get(application.PledgeId);

            application.WithdrawAfterAcceptance(userInfo);
            pledge.Credit(application.Amount, userInfo);

            return Unit.Value;
        }
    }
}
