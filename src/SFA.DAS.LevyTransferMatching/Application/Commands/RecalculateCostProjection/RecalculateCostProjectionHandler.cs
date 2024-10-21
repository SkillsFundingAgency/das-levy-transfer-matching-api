using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateCostProjection;

public class RecalculateCostProjectionHandler(IApplicationRepository repository, ICostProjectionService costProjectionService)
    : IRequestHandler<RecalculateCostProjectionCommand>
{
    public async Task Handle(RecalculateCostProjectionCommand request, CancellationToken cancellationToken)
    {
        var application = await repository.Get(request.ApplicationId);

        var costProjections = costProjectionService.GetCostProjections(application.StandardMaxFunding * application.NumberOfApprentices, application.StartDate, application.StandardDuration);

        application.SetCostProjection(costProjections);

        await repository.Update(application);
    }
}