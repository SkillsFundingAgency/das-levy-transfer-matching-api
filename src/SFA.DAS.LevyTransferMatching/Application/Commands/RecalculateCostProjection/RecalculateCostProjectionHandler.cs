using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateCostProjection
{
    public class RecalculateCostProjectionHandler : IRequestHandler<RecalculateCostProjectionCommand>
    {
        private readonly IApplicationRepository _repository;
        private readonly ICostProjectionService _costProjectionService;

        public RecalculateCostProjectionHandler(IApplicationRepository repository, ILogger<RecalculateCostProjectionHandler> logger, ICostProjectionService costProjectionService)
        {
            _repository = repository;
            _costProjectionService = costProjectionService;
        }

        public async Task<Unit> Handle(RecalculateCostProjectionCommand request, CancellationToken cancellationToken)
        {
            var application = await _repository.Get(request.ApplicationId);

            var costProjections = _costProjectionService.GetCostProjections(application.StandardMaxFunding, application.StartDate, application.StandardDuration);

            application.SetCostProjection(costProjections);
            
            await _repository.Update(application);

            return Unit.Value;
        }
    }
}