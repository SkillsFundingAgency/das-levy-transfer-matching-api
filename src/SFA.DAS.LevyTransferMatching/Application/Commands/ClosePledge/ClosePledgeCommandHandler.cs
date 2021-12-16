using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommandHandler : IRequestHandler<ClosePledgeCommand, ClosePledgeResult>
    {
     
        private readonly IPledgeRepository _pledgeRepository;
        private readonly ILogger<ClosePledgeCommandHandler> _logger;

        public ClosePledgeCommandHandler(IPledgeRepository pledgeRepository, ILogger<ClosePledgeCommandHandler> logger)
        {
            _pledgeRepository = pledgeRepository;
            _logger = logger;
        }

        public async Task<ClosePledgeResult> Handle(ClosePledgeCommand request, CancellationToken cancellationToken)
        {

            if (request == null)
            {
               return new ClosePledgeResult
                {
                    Updated = false,
                    Message = $"The pledge for {request} could not be found."
                };
            }

            var pledge = await _pledgeRepository.Get(request.PledgeId);

            if (pledge == null)
            {
                _logger.LogInformation($"The pledge for {request} could not be found.");

                return new ClosePledgeResult
                {
                    Updated = false,
                    Message = $"The pledge for {request.PledgeId} could not be found."
                };
            }

            pledge.ClosePledge();
            await _pledgeRepository.Update(pledge);

            return new ClosePledgeResult
            {
                Updated = true,
                Message = $"The pledge Id = {request.PledgeId} is closed."
            };
            
        }
    }
}
