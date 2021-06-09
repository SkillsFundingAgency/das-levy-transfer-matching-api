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

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            command.AccountId = _hashingService.DecodeValue(command.EncodedAccountId);

            await _pledgesDataRepository.Add(command);

            return new CreatePledgeResult()
            {
                Id = command.Id.Value,
            };
        }
    }
}