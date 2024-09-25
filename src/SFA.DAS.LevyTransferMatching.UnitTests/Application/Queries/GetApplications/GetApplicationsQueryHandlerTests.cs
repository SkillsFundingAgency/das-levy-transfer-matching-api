using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.Services;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplications;

[TestFixture]
public class GetApplicationsQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;
    private GetApplicationsQueryHandler _handler;
    private Mock<IDateTimeService> _dateTimeService;

    [SetUp]
    public async Task Setup()
    {
        _fixture = new Fixture();

        var sendingEmployer = _fixture.Create<EmployerAccount>();
        var receivingEmployer = _fixture.Create<EmployerAccount>();

        await DbContext.EmployerAccounts.AddAsync(sendingEmployer);
        await DbContext.EmployerAccounts.AddAsync(receivingEmployer);

        var pledge = sendingEmployer.CreatePledge(
            _fixture.Create<CreatePledgeProperties>(),
            _fixture.Create<UserInfo>());

        await DbContext.Pledges.AddAsync(pledge);

        var applications = new List<LevyTransferMatching.Data.Models.Application>();

        _dateTimeService = new Mock<IDateTimeService>();
        _dateTimeService.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        for (var index = 0; index < 20; index++)
        {
            var properties = _fixture.Build<CreateApplicationProperties>()
                .With(x => x.CostProjections, () => new List<CostProjection>{ new CostProjection(DateTime.UtcNow.GetFinancialYear(), _fixture.Create<int>())})
                .With(x => x.Locations, () => new List<int>())
                .With(x => x.EmailAddresses, () => new List<string>{"test@test.com"})
                .Create();

            var application = pledge.CreateApplication(receivingEmployer, properties, UserInfo.System);

            applications.Add(application);
        }

        await DbContext.Applications.AddRangeAsync(applications);

        await DbContext.SaveChangesAsync();

        _handler = new GetApplicationsQueryHandler(DbContext, _dateTimeService.Object);
    }
    
    [Test]
    public async Task Handle_Item_Count_Is_Correct()
    {
        var request = new GetApplicationsQuery();
        var result = await _handler.Handle(request, CancellationToken.None);

        result.TotalItems.Should().Be(20);
    }

    [Test]
    public async Task Handle_When_Application_CostingModel_Is_OneYear_Then_Amount_Is_Correct()
    {
        var request = new GetApplicationsQuery();

        foreach (var expected in DbContext.Applications)
        {
            expected.SetValue(x=> x.CostingModel, ApplicationCostingModel.OneYear);
        }
        await DbContext.SaveChangesAsync();

        var result = await _handler.Handle(request, CancellationToken.None);

        foreach (var expected in DbContext.Applications)
        {
            var actual = result.Items.Single(x => x.Id == expected.Id);

            actual.Amount.Should().Be(expected.GetCost());
        }
    }

    [TestCase(10, 2)]
    [TestCase(5, 4)]
    [TestCase(3, 7)]
    public async Task Handle_Paging_Options_Are_Reflected_In_Results(int? pageSize, int expectedPages)
    {
        var request = new GetApplicationsQuery
        {
            PageSize = pageSize
        };
        var result = await _handler.Handle(request, CancellationToken.None);

        result.TotalItems.Should().Be(20);
        result.TotalPages.Should().Be(expectedPages);
        result.Items.Count.Should().BeLessThanOrEqualTo(pageSize ?? int.MaxValue);
    }

    [Test]
    public async Task Handle_SortBy_ApplicationDate_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery{ SortOrder = GetApplicationsSortOrder.ApplicationDate, SortDirection = SortDirection.Descending};
        var result = await _handler.Handle(request, CancellationToken.None);
            
        var expectedSequence = DbContext.Applications
            .OrderByDescending(x => x.CreatedOn)
            .ThenBy(x=> x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x => x.Id).ToArray());
    }

    [Test]
    public async Task Handle_SortBy_CriteriaMatch_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery { SortOrder = GetApplicationsSortOrder.CriteriaMatch, SortDirection = SortDirection.Ascending };
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedSequence = DbContext.Applications
            .OrderBy(x => x.MatchPercentage)
            .ThenBy(x => x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x => x.Id).ToArray());
    }

    [Test]
    public async Task Handle_SortBy_Applicant_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery { SortOrder = GetApplicationsSortOrder.Applicant, SortDirection = SortDirection.Ascending };
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedSequence = DbContext.Applications
            .OrderBy(x => x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x=> x.Id).ToArray());
    }

    [Test]
    public async Task Handle_SortBy_Duration_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery { SortOrder = GetApplicationsSortOrder.Duration, SortDirection = SortDirection.Ascending };
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedSequence = DbContext.Applications
            .OrderBy(x => x.StandardDuration)
            .ThenBy(x => x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x => x.Id).ToArray());
    }

    [Test]
    public async Task Handle_SortBy_CurrentFinancialYearAmount_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery { SortOrder = GetApplicationsSortOrder.CurrentFinancialYearAmount, SortDirection = SortDirection.Descending };
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedSequence = DbContext.Applications
            .OrderByDescending(x => x.ApplicationCostProjections.Where(p=> p.FinancialYear == DateTime.UtcNow.GetFinancialYear()).Sum(p=> p.Amount))
            .ThenBy(x => x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x => x.Id).ToArray());
    }

    [Test]
    public async Task Handle_SortBy_Status_Produces_ExpectedResult()
    {
        var request = new GetApplicationsQuery { SortOrder = GetApplicationsSortOrder.Status, SortDirection = SortDirection.Ascending };
        var result = await _handler.Handle(request, CancellationToken.None);

        var expectedSequence = DbContext.Applications
            .OrderBy(x => x.Status.ToString())
            .ThenBy(x => x.EmployerAccount.Name)
            .Select(x => x.Id).ToArray();

        AssertCorrectOrder(expectedSequence, result.Items.Select(x => x.Id).ToArray());
    }

    private static void AssertCorrectOrder(IReadOnlyList<int> expected, IReadOnlyList<int> actual)
    {
        actual.Count.Should().Be(expected.Count);

        for (var index = 0; index < expected.Count; index++)
        {
            actual[index].Should().Be(expected[index]);
        }
    }
}