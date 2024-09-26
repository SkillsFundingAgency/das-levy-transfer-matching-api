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

        _inserted.Should().NotBeNull();
        _inserted.Pledge.Id.Should().Be(command.PledgeId);
        _inserted.EmployerAccount.Id.Should().Be(command.EmployerAccountId);
        _inserted.Details.Should().Be(command.Details);
        _inserted.StandardId.Should().Be(command.StandardId);
        _inserted.StandardTitle.Should().Be(command.StandardTitle);
        _inserted.StandardLevel.Should().Be(command.StandardLevel);
        _inserted.StandardDuration.Should().Be(command.StandardDuration);
        _inserted.StandardMaxFunding.Should().Be(command.StandardMaxFunding);
        _inserted.StandardRoute.Should().Be(command.StandardRoute);
        _inserted.NumberOfApprentices.Should().Be(command.NumberOfApprentices);
        _inserted.StartDate.Should().Be(command.StartDate);
        _inserted.HasTrainingProvider.Should().Be(command.HasTrainingProvider);
        _inserted.Sectors.Should().Be((Sector)command.Sectors.Cast<int>().Sum());
        _inserted.ApplicationLocations.Select(x => x.PledgeLocationId).ToList().Should().BeEquivalentTo(command.Locations);
        _inserted.AdditionalLocation.Should().Be(command.AdditionalLocation);
        _inserted.SpecificLocation.Should().Be(command.SpecificLocation);
        _inserted.FirstName.Should().Be(command.FirstName);
        _inserted.LastName.Should().Be(command.LastName);
        _inserted.BusinessWebsite.Should().Be(command.BusinessWebsite);
        _inserted.TotalAmount.Should().Be(command.NumberOfApprentices * command.StandardMaxFunding);
        _inserted.EmailAddresses.Select(x => x.EmailAddress).Should().BeEquivalentTo(command.EmailAddresses);
        CompareHelper.AreEqualIgnoringTypes(_costProjections, _inserted.ApplicationCostProjections);
        _inserted.MatchJobRole.Should().Be(_matchingCriteria.MatchJobRole);
        _inserted.MatchLevel.Should().Be(_matchingCriteria.MatchLevel);
        _inserted.MatchLocation.Should().Be(_matchingCriteria.MatchLocation);
        _inserted.MatchSector.Should().Be(_matchingCriteria.MatchSector);
        _inserted.MatchPercentage.Should().Be(_matchingCriteria.MatchPercentage);
        _inserted.CostingModel.Should().Be(ApplicationCostingModel.Original);
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

        _inserted.CostingModel.Should().Be(ApplicationCostingModel.OneYear);
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
            : (fundingBandMax * 0.8m / command.StandardDuration * 12 * command.NumberOfApprentices).ToNearest(1);

        _inserted.GetCost().Should().Be((int)expected);
    }
}