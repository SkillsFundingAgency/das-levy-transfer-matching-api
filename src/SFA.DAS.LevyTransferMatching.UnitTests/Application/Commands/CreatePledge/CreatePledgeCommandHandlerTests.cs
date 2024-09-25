using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using KellermanSoftware.CompareNetObjects;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge;

[TestFixture]
public class CreatePledgeCommandHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;
    private Mock<IEmployerAccountRepository> _employerAccountRepository;
    private Mock<IPledgeRepository> _pledgeRepository;

    private CreatePledgeCommandHandler _handler;

    private EmployerAccount _employerAccount;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();

        _employerAccountRepository = new Mock<IEmployerAccountRepository>();
        _pledgeRepository = new Mock<IPledgeRepository>();

        _employerAccount = _fixture.Create<EmployerAccount>();

        _employerAccountRepository.Setup(x => x.Get(_employerAccount.Id)).ReturnsAsync(_employerAccount);

        _handler = new CreatePledgeCommandHandler(_employerAccountRepository.Object, _pledgeRepository.Object);

    }

    [Test]
    public async Task Handle_Pledge_Is_Created()
    {
        var command = _fixture.Create<CreatePledgeCommand>();
        command.AccountId = _employerAccount.Id;

        Pledge inserted = null;

        _pledgeRepository.Setup(x => x.Add(It.IsAny<Pledge>()))
            .Callback<Pledge>(r => inserted = r);

        await _handler.Handle(command, CancellationToken.None);

        inserted.Should().NotBeNull();
        Assert.That(inserted.EmployerAccount.Id, Is.EqualTo(command.AccountId));
        Assert.That(inserted.Amount, Is.EqualTo(command.Amount));
        Assert.That(inserted.RemainingAmount, Is.EqualTo(command.Amount));
        Assert.That(inserted.IsNamePublic, Is.EqualTo(command.IsNamePublic));
        Assert.That(inserted.AutomaticApprovalOption, Is.EqualTo(command.AutomaticApprovalOption));
        Assert.That(inserted.Levels, Is.EqualTo((Level)command.Levels.Cast<int>().Sum()));
        Assert.That(inserted.Sectors, Is.EqualTo((Sector)command.Sectors.Cast<int>().Sum()));
        Assert.That(inserted.JobRoles, Is.EqualTo((JobRole)command.JobRoles.Cast<int>().Sum()));

        var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreCollectionOrder = true, IgnoreObjectTypes = true, IgnoreUnknownObjectTypes = true });
        var expectedLocations = command.Locations.Select(l => new PledgeLocation { Name = l.Name, Latitude = l.Geopoint[0], Longitude = l.Geopoint[1] }).ToList();
        var result = compareLogic.Compare(expectedLocations, inserted.Locations);
        result.AreEqual.Should().BeTrue(result.DifferencesString);
    }
}