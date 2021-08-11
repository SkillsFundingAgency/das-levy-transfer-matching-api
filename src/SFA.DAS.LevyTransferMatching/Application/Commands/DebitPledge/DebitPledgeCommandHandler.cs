using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommandHandler : IRequestHandler<DebitPledgeCommand>
    {
        private readonly IPledgeRepository _repository;

        public DebitPledgeCommandHandler(IPledgeRepository repository)
        {
            _repository = repository;
        }

        public Task<Unit> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}