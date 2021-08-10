using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ApproveApplicationCommandHandler : IRequestHandler<ApproveApplicationCommand>
    {
        public Task<Unit> Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}