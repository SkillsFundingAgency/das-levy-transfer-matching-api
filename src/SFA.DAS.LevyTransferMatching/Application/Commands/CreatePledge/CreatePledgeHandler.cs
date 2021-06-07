using MediatR;
using SFA.DAS.HashingService;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly IHashingService _hashingService;
        private readonly IPledgesDataRepository _pledgesDataRepository;

        public CreatePledgeHandler(IHashingService hashingService, IPledgesDataRepository pledgesDataRepository)
        {
            _hashingService = hashingService;
            _pledgesDataRepository = pledgesDataRepository;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand request, CancellationToken cancellationToken)
        {
            Pledge pledge = request.Pledge;

            pledge.AccountId = _hashingService.DecodeValue(pledge.EncodedAccountId);

            await _pledgesDataRepository.Add(pledge);

            return new CreatePledgeResult()
            {
                Id = pledge.Id.Value,
            };
        }
    }
}