using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommandHandler : IRequestHandler<DebitPledgeCommand>
    {
        private readonly IPledgeRepository _repository;

        public DebitPledgeCommandHandler(IPledgeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
        {
            var pledge = await _repository.Get(request.PledgeId);

            pledge.Debit(request.Amount, UserInfo.System);

            await _repository.Update(pledge);

            return Unit.Value;
        }
    }
}