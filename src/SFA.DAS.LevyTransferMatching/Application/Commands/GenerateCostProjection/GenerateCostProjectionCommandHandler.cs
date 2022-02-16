using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.GenerateCostProjection
{
    public class GenerateCostProjectionCommandHandler : IRequestHandler<GenerateCostProjectionCommand>
    {
        private readonly IApplicationRepository _repository;
        private readonly ILogger<GenerateCostProjectionCommandHandler> _logger;
        private readonly ICostProjectionService _costProjectionService;

        public GenerateCostProjectionCommandHandler(IApplicationRepository repository, ILogger<GenerateCostProjectionCommandHandler> logger, ICostProjectionService costProjectionService)
        {
            _repository = repository;
            _logger = logger;
            _costProjectionService = costProjectionService;
        }

        public async Task<Unit> Handle(GenerateCostProjectionCommand request, CancellationToken cancellationToken)
        {
            var application = await _repository.Get(request.ApplicationId);

            if (application.ApplicationCostProjections.Any())
            {
                throw new InvalidOperationException($"Unable to Add Cost Projections for Application {application.Id}; projections already exist");
            }

            var costProjections = _costProjectionService.GetCostProjections(
                application.StandardMaxFunding * application.NumberOfApprentices, application.StartDate,
                application.StandardDuration);

            application.AddCostProjections(costProjections);
            
            await _repository.Update(application);

            return Unit.Value;
        }
    }
}