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
    public class GenerateMatchingCriteriaCommandHandler : IRequestHandler<GenerateMatchingCriteria>
    {
        private readonly IApplicationRepository _repository;
        private readonly IPledgeRepository _pledgeRepository;
        private readonly ILogger<GenerateMatchingCriteriaCommandHandler> _logger;
        private readonly IMatchingCriteriaService _matchingCriteriaService;

        public GenerateMatchingCriteriaCommandHandler(IApplicationRepository repository, ILogger<GenerateMatchingCriteriaCommandHandler> logger, IMatchingCriteriaService matchingCriteriaService, IPledgeRepository pledgeRepository)
        {
            _repository = repository;
            _logger = logger;
            _matchingCriteriaService = matchingCriteriaService;
            _pledgeRepository = pledgeRepository;
        }

        public async Task<Unit> Handle(GenerateMatchingCriteria request, CancellationToken cancellationToken)
        {
            var application = await _repository.Get(request.ApplicationId);
            var pledge = await _pledgeRepository.Get(application.PledgeId);

            if (application.MatchPercentage != 255)
            {
                throw new InvalidOperationException($"Unable to Add Matching Criteria for Application {application.Id}; criteria has already been added");
            }

            var matchingCriteria = _matchingCriteriaService.GetMatchingCriteria(application, pledge);

            application.AddMatchingCriteria(matchingCriteria);
            
            await _repository.Update(application);

            return Unit.Value;
        }
    }
}