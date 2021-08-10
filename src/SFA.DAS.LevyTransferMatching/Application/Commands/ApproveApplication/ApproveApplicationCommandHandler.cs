using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ApproveApplicationCommandHandler : IRequestHandler<ApproveApplicationCommand>
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApproveApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public Task<Unit> Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}