using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateCostProjection;

public class RecalculateCostProjectionHandler : IRequestHandler<RecalculateCostProjectionCommand>
{
    private readonly IApplicationRepository _repository;
    private readonly ICostProjectionService _costProjectionService;

    public RecalculateCostProjectionHandler(IApplicationRepository repository, ICostProjectionService costProjectionService)
    {
        _repository = repository;
        _costProjectionService = costProjectionService;
    }

    public async Task Handle(RecalculateCostProjectionCommand request, CancellationToken cancellationToken)
    {
        var application = await _repository.Get(request.ApplicationId);

        var costProjections = _costProjectionService.GetCostProjections(application.StandardMaxFunding * application.NumberOfApprentices, application.StartDate, application.StandardDuration);

        application.SetCostProjection(costProjections);

        await _repository.Update(application);
    }
}