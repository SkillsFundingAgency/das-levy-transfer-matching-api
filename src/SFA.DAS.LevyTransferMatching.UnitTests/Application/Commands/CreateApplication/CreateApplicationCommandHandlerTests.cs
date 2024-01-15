using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Services;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateApplication;

[TestFixture]
public class CreateApplicationCommandHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;
    private Mock<IEmployerAccountRepository> _employerAccountRepository;
    private Mock<IPledgeRepository> _pledgeRepository;
    private Mock<IApplicationRepository> _applicationRepository;
    private Mock<ICostProjectionService> _costProjectionService;
    private Mock<IMatchingCriteriaService> _matchingCriteriaService;
    private FeatureToggles _featureToggles;

    private CreateApplicationCommandHandler _handler;

    private EmployerAccount _employerAccount;
    private Pledge _pledge;
    private List<CostProjection> _costProjections;
    private MatchingCriteria _matchingCriteria;

    private Data.Models.Application _inserted;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();

        _employerAccountRepository = new Mock<IEmployerAccountRepository>();
        _pledgeRepository = new Mock<IPledgeRepository>();
        _applicationRepository = new Mock<IApplicationRepository>();
        _costProjectionService = new Mock<ICostProjectionService>();
        _matchingCriteriaService = new Mock<IMatchingCriteriaService>();

        _employerAccount = _fixture.Create<EmployerAccount>();
        _pledge = _fixture.Create<Pledge>();
        _costProjections = _fixture.Create<List<CostProjection>>();
        _matchingCriteria = _fixture.Create<MatchingCriteria>();

        _featureToggles = new FeatureToggles { ToggleNewCostingModel = false };

        _employerAccountRepository.Setup(x => x.Get(_employerAccount.Id)).ReturnsAsync(_employerAccount);
        _pledgeRepository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

        _applicationRepository.Setup(x => x.Add(It.IsAny<LevyTransferMatching.Data.Models.Application>()))
            .Callback<LevyTransferMatching.Data.Models.Application>(r => _inserted = r);

        _costProjectionService
            .Setup(x => x.GetCostProjections(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Returns(_costProjections);

        _matchingCriteriaService.Setup(x =>
                x.GetMatchingCriteria(It.IsAny<CreateApplicationCommand>(), It.IsAny<Pledge>()))
            .Returns(_matchingCriteria);

        _handler = new CreateApplicationCommandHandler(_pledgeRepository.Object, _applicationRepository.Object, _employerAccountRepository.Object, _costProjectionService.Object, _matchingCriteriaService.Object, _featureToggles);
    }

    [Test]
    public async Task Handle_Application_Is_Created()
    {
        var command = _fixture.Create<CreateApplicationCommand>();
        command.EmployerAccountId = _employerAccount.Id;
        command.PledgeId = _pledge.Id;
        command.Locations = _pledge.Locations.Select(x => x.Id).ToList();

        await _handler.Handle(command, CancellationToken.None);

        Assert.That(_inserted, Is.Not.Null);
        Assert.That(_inserted.Pledge.Id, Is.EqualTo(command.PledgeId));
        Assert.That(_inserted.EmployerAccount.Id, Is.EqualTo(command.EmployerAccountId));
        Assert.That(_inserted.Details, Is.EqualTo(command.Details));
        Assert.That(_inserted.StandardId, Is.EqualTo(command.StandardId));
        Assert.That(_inserted.StandardTitle, Is.EqualTo(command.StandardTitle));
        Assert.That(_inserted.StandardLevel, Is.EqualTo(command.StandardLevel));
        Assert.That(_inserted.StandardDuration, Is.EqualTo(command.StandardDuration));
        Assert.That(_inserted.StandardMaxFunding, Is.EqualTo(command.StandardMaxFunding));
        Assert.That(_inserted.StandardRoute, Is.EqualTo(command.StandardRoute));
        Assert.That(_inserted.NumberOfApprentices, Is.EqualTo(command.NumberOfApprentices));
        Assert.That(_inserted.StartDate, Is.EqualTo(command.StartDate));
        Assert.That(_inserted.HasTrainingProvider, Is.EqualTo(command.HasTrainingProvider));
        Assert.That(_inserted.Sectors, Is.EqualTo((Sector)command.Sectors.Cast<int>().Sum()));
        Assert.That(_inserted.ApplicationLocations.Select(x => x.PledgeLocationId).ToList(), Is.EqualTo(command.Locations).AsCollection);
        Assert.That(_inserted.AdditionalLocation, Is.EqualTo(command.AdditionalLocation));
        Assert.That(_inserted.SpecificLocation, Is.EqualTo(command.SpecificLocation));
        Assert.That(_inserted.FirstName, Is.EqualTo(command.FirstName));
        Assert.That(_inserted.LastName, Is.EqualTo(command.LastName));
        Assert.That(_inserted.BusinessWebsite, Is.EqualTo(command.BusinessWebsite));
        Assert.That(_inserted.TotalAmount, Is.EqualTo(command.NumberOfApprentices * command.StandardMaxFunding));
        Assert.That(_inserted.EmailAddresses.Select(x => x.EmailAddress), Is.EqualTo(command.EmailAddresses).AsCollection);
        CompareHelper.AreEqualIgnoringTypes(_costProjections, _inserted.ApplicationCostProjections);
        Assert.That(_inserted.MatchJobRole, Is.EqualTo(_matchingCriteria.MatchJobRole));
        Assert.That(_inserted.MatchLevel, Is.EqualTo(_matchingCriteria.MatchLevel));
        Assert.That(_inserted.MatchLocation, Is.EqualTo(_matchingCriteria.MatchLocation));
        Assert.That(_inserted.MatchSector, Is.EqualTo(_matchingCriteria.MatchSector));
        Assert.That(_inserted.MatchPercentage, Is.EqualTo(_matchingCriteria.MatchPercentage));
        Assert.That(_inserted.CostingModel, Is.EqualTo(ApplicationCostingModel.Original));
    }

    [Test]
    public async Task Handle_When_New_Costing_Model_Is_Toggled_On_Application_Is_Created_With_OneYear_CostingModel()
    {
        var command = _fixture.Create<CreateApplicationCommand>();
        command.EmployerAccountId = _employerAccount.Id;
        command.PledgeId = _pledge.Id;
        command.Locations = _pledge.Locations.Select(x => x.Id).ToList();

        _featureToggles.ToggleNewCostingModel = true;

        await _handler.Handle(command, CancellationToken.None);

        Assert.That(_inserted.CostingModel, Is.EqualTo(ApplicationCostingModel.OneYear));
    }

    [Test]
    public async Task Handle_When_New_Costing_Model_Is_Toggled_On_Application_Is_Created_With_Amount_Equal_To_One_Year_Cost()
    {
        var command = _fixture.Create<CreateApplicationCommand>();
        command.EmployerAccountId = _employerAccount.Id;
        command.PledgeId = _pledge.Id;
        command.Locations = _pledge.Locations.Select(x => x.Id).ToList();

        _featureToggles.ToggleNewCostingModel = true;

        await _handler.Handle(command, CancellationToken.None);

        var fundingBandMax = (decimal)command.StandardMaxFunding;

        var expected = command.StandardDuration <= 12
            ? fundingBandMax * command.NumberOfApprentices
            : (((fundingBandMax * 0.8m) / command.StandardDuration) * 12 * command.NumberOfApprentices).ToNearest(1);

        Assert.That(_inserted.GetCost(), Is.EqualTo(expected));
    }
}