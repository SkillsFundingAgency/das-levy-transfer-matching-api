using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Services;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services;

[TestFixture]
public class MatchingCriteriaServiceTests
{
    private MatchingCriteriaService _service;
    private CreateApplicationCommand _application;
    private Pledge _pledge;
    private const Sector AllSectors = 0;
    private const Sector CreativeOrConstructionSector = (Sector)96;
    private const JobRole AllJobRoles = 0;
    private const JobRole AgricultureOrBusinessJobRole = (JobRole)3;
    private const Level AllLevels = 0;
    private const Level Level3Or4 = (Level)6;

    [SetUp]
    public void Setup()
    {
        _service = new MatchingCriteriaService();

        _application = new CreateApplicationCommand
        {
            Locations = [],
            Sectors = new List<Sector>(),
            StandardRoute = "",
            StandardLevel = 0
        };

        var account = EmployerAccount.New(1, "Test");
        _pledge = new Pledge(account, new CreatePledgeProperties(), UserInfo.System);
    }

    [TestCase(AllSectors, Sector.Creative, true)]
    [TestCase(Sector.Creative, Sector.Creative, true)]
    [TestCase(CreativeOrConstructionSector, Sector.Creative, true)]
    [TestCase(Sector.Creative, CreativeOrConstructionSector, true)]
    [TestCase(CreativeOrConstructionSector, CreativeOrConstructionSector, true)]
    [TestCase(Sector.Creative, Sector.Agriculture, false)]
    [TestCase(CreativeOrConstructionSector, Sector.Agriculture, false)]
    public void GetMatchingCriteria_Sector_Matches(Sector pledgeSector, Sector applicationSector, bool expectMatch)
    {
        _pledge.SetValue(x => x.Sectors, pledgeSector);
        _application.Sectors = applicationSector.ToList();

        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchSector.Should().Be(expectMatch);
    }

    [Test]
    public void GetMatchingCriteria_Location_Matches_If_Pledge_Location_Is_All_Of_England()
    {
        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchLocation.Should().BeTrue();
    }

    [Test]
    public void GetMatchingCriteria_Location_Matches_If_Application_Has_Any_Location_Specified()
    {
        _application.Locations.Add(1);
        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchLocation.Should().BeTrue();
    }

    [Test]
    public void GetMatchingCriteria_Location_Does_Not_Match_If_Pledge_Is_Not_All_Of_England_And_Application_Has_No_Location_Specified()
    {
        var pledgeLocations = new List<PledgeLocation> { new() { Name = "A specific place in England" } };
        _pledge = new Pledge(EmployerAccount.New(1, "Test"), new CreatePledgeProperties { Locations = pledgeLocations }, UserInfo.System);

        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchLocation.Should().BeFalse();
    }

    [TestCase(AllJobRoles, "Agriculture, environmental and animal care", true)]
    [TestCase(JobRole.Agriculture, "Agriculture, environmental and animal care", true)]
    [TestCase(AgricultureOrBusinessJobRole, "Agriculture, environmental and animal care", true)]
    [TestCase(JobRole.Agriculture, "Construction", false)]
    [TestCase(AgricultureOrBusinessJobRole, "Construction", false)]
    public void GetMatchingCriteria_JobRole_Matches(JobRole pledgeJobRole, string applicationRoute, bool expectMatch)
    {
        _pledge.SetValue(x => x.JobRoles, pledgeJobRole);
        _application.StandardRoute = applicationRoute;
        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchJobRole.Should().Be(expectMatch);
    }

    [TestCase(AllLevels, 1, true)]
    [TestCase(Level.Level3, 3, true)]
    [TestCase(Level3Or4, 3, true)]
    [TestCase(Level.Level3, 1, false)]
    [TestCase(Level3Or4, 1, false)]
    public void GetMatchingCriteria_Level_Matches(Level pledgeLevel, int applicationLevel, bool expectMatch)
    {
        _pledge.SetValue(x => x.Levels, pledgeLevel);
        _application.StandardLevel = applicationLevel;

        var result = _service.GetMatchingCriteria(_application, _pledge);
        result.MatchLevel.Should().Be(expectMatch);
    }
}