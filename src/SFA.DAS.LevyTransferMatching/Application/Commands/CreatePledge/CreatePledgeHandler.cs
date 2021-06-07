using MediatR;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly IPledgesDataRepository _pledgesDataRepository;

        public CreatePledgeHandler(IPledgesDataRepository pledgesDataRepository)
        {
            _pledgesDataRepository = pledgesDataRepository;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            Pledge pledge = request.Pledge;

            // TODO: Find hashing NuGet package to unpack the id.
            pledge.AccountId = 0;

            await _pledgesDataRepository.Add(pledge);

            return new CreatePledgeResult()
            {
                Id = pledge.Id.Value,
            };
        }
    }
}