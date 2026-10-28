using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplicationsForAutomaticApproval;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplicationsForAutomaticApproval;

[TestFixture]
public class GetApplicationsForAutomaticApprovalQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private GetApplicationsForAutomaticApprovalQueryHandler _handler;
    private int _firstPledgeId;
    private int _secondPledgeId;

    [SetUp]
    public async Task Setup()
    {
        var fixture = new Fixture();
        var sendingEmployer = fixture.Create<EmployerAccount>();
        var receivingEmployer = fixture.Create<EmployerAccount>();
        await DbContext.EmployerAccounts.AddRangeAsync(sendingEmployer, receivingEmployer);

        var firstPledge = sendingEmployer.CreatePledge(fixture.Create<CreatePledgeProperties>(), UserInfo.System);
        var secondPledge = sendingEmployer.CreatePledge(fixture.Create<CreatePledgeProperties>(), UserInfo.System);
        await DbContext.Pledges.AddRangeAsync(firstPledge, secondPledge);

        var pendingOnFirstPledge = firstPledge.CreateApplication(receivingEmployer, ApplicationProperties(fixture), UserInfo.System);
        var approvedOnFirstPledge = firstPledge.CreateApplication(receivingEmployer, ApplicationProperties(fixture), UserInfo.System);
        approvedOnFirstPledge.SetValue(application => application.Status, ApplicationStatus.Approved);
        var pendingOnSecondPledge = secondPledge.CreateApplication(receivingEmployer, ApplicationProperties(fixture), UserInfo.System);

        await DbContext.Applications.AddRangeAsync(pendingOnFirstPledge, approvedOnFirstPledge, pendingOnSecondPledge);
        await DbContext.SaveChangesAsync();

        _firstPledgeId = firstPledge.Id;
        _secondPledgeId = secondPledge.Id;
        _handler = new GetApplicationsForAutomaticApprovalQueryHandler(DbContext);
    }

    [Test]
    public async Task Handle_Returns_Pending_Applications_For_The_Pledge()
    {
        var result = await _handler.Handle(new GetApplicationsForAutomaticApprovalQuery
        {
            PledgeId = _firstPledgeId
        }, CancellationToken.None);

        result.Applications.Should().ContainSingle();
        result.Applications.Single().PledgeId.Should().Be(_firstPledgeId);
        result.Applications.Single().Status.Should().Be(ApplicationStatus.Pending.ToString());
    }

    [Test]
    public async Task Handle_Without_A_Pledge_Returns_Pending_Applications_Only()
    {
        var result = await _handler.Handle(new GetApplicationsForAutomaticApprovalQuery(), CancellationToken.None);

        result.Applications.Select(application => application.PledgeId).Should().BeEquivalentTo(new[] { _firstPledgeId, _secondPledgeId });
        result.Applications.Should().OnlyContain(application => application.Status == ApplicationStatus.Pending.ToString());
    }

    private static CreateApplicationProperties ApplicationProperties(Fixture fixture)
    {
        return fixture.Build<CreateApplicationProperties>()
            .With(properties => properties.CostProjections, () => new List<CostProjection> { new(DateTime.UtcNow.GetFinancialYear(), fixture.Create<int>()) })
            .With(properties => properties.Locations, () => new List<int>())
            .With(properties => properties.EmailAddresses, () => new List<string> { "test@test.com" })
            .Create();
    }
}
