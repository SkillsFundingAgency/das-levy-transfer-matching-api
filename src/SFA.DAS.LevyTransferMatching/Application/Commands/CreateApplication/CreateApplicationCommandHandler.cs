using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
{
    private readonly IPledgeRepository _pledgeRepository;
    private readonly IEmployerAccountRepository _employerAccountRepository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICostProjectionService _costProjectionService;
    private readonly IMatchingCriteriaService _matchingCriteriaService;
    private readonly FeatureToggles _featureToggles;

    public CreateApplicationCommandHandler(IPledgeRepository pledgeRepository,
        IApplicationRepository applicationRepository,
        IEmployerAccountRepository employerAccountRepository,
        ICostProjectionService costProjectionService, IMatchingCriteriaService matchingCriteriaService, FeatureToggles featureToggles)
    {
        _pledgeRepository = pledgeRepository;
        _applicationRepository = applicationRepository;
        _employerAccountRepository = employerAccountRepository;
        _costProjectionService = costProjectionService;
        _matchingCriteriaService = matchingCriteriaService;
        _featureToggles = featureToggles;
    }

    public async Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _employerAccountRepository.Get(request.EmployerAccountId);
        var pledge = await _pledgeRepository.Get(request.PledgeId);

        var costProjections = _costProjectionService.GetCostProjections(request.StandardMaxFunding * request.NumberOfApprentices, request.StartDate, request.StandardDuration);

        var matchingCriteria = _matchingCriteriaService.GetMatchingCriteria(request, pledge);

        var settings = new CreateApplicationProperties
        {
            Details = request.Details,
            StandardId = request.StandardId,
            StandardTitle = request.StandardTitle,
            StandardLevel = request.StandardLevel,
            StandardDuration = request.StandardDuration,
            StandardMaxFunding = request.StandardMaxFunding,
            StandardRoute = request.StandardRoute,
            NumberOfApprentices = request.NumberOfApprentices,
            StartDate = request.StartDate,
            HasTrainingProvider = request.HasTrainingProvider,
            Sectors = (Sector)request.Sectors.Cast<int>().Sum(),
            Locations = request.Locations,
            AdditionalLocation = request.AdditionalLocation,
            SpecificLocation = request.SpecificLocation,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BusinessWebsite = request.BusinessWebsite,
            EmailAddresses = request.EmailAddresses,
            CostProjections = costProjections,
            MatchingCriteria = matchingCriteria,
            CostingModel = _featureToggles.ToggleNewCostingModel
                ? ApplicationCostingModel.OneYear
                : ApplicationCostingModel.Original
        };

        var application = pledge.CreateApplication(account, settings, new UserInfo(request.UserId, request.UserDisplayName));

        await _applicationRepository.Add(application);

        return new CreateApplicationCommandResult
        {
            ApplicationId = application.Id
        };
    }
}