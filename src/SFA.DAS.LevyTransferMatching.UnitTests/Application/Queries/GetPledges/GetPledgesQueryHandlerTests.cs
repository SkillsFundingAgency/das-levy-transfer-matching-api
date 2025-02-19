﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges;

[TestFixture]
public class GetPledgesQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;

    private static readonly object[] SectorLists =
    [
        new object[] { new List<Sector> { Sector.Agriculture } },
        new object[] { new List<Sector> { Sector.Business, Sector.Charity } },
        new object[] { new List<Sector> { Sector.Education, Sector.Digital, Sector.Construction, Sector.Legal } },
        new object[] { new List<Sector> { Sector.Health, Sector.ProtectiveServices } },
        new object[] { new List<Sector> { Sector.Sales, Sector.Transport, Sector.CareServices, Sector.Catering } }
    ];

    [SetUp]
    public async Task Setup()
    {
        _fixture = new Fixture();

        var employerAccounts = _fixture.CreateMany<EmployerAccount>(10).ToArray();

        await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

        var pledges = new List<Pledge>();

        for (var i = 0; i < employerAccounts.Length; i++)
        {
            pledges.Add(
                employerAccounts[i].CreatePledge(
                    _fixture.Create<CreatePledgeProperties>(),
                    _fixture.Create<UserInfo>()
                ));
        }

        await DbContext.Pledges.AddRangeAsync(pledges);

        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Handle_All_Pledges_Pulled_And_Stitched_Up_With_Accounts()
    {
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.ValueHighToLow
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        var actualPledges = result.Items.ToArray();

        // Assert
        var dbPledges = await DbContext.Pledges.OrderByDescending(x => x.RemainingAmount).ToArrayAsync();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            dbPledges[index].Id.Should().Be(actualPledges[index].Id);
            dbPledges[index].EmployerAccount.Id.Should().Be(actualPledges[index].AccountId);
        }
    }

    [TestCase(null, 1)]
    [TestCase(10, 1)]
    [TestCase(5, 2)]
    [TestCase(3, 4)]
    public async Task Handle_Paging_Options_Are_Reflected_In_Results(int? pageSize, int expectedPages)
    {
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            PageSize = pageSize
        };

        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        result.TotalItems.Should().Be(10);
        result.TotalPages.Should().Be(expectedPages);
        result.Items.Count.Should().Be(pageSize ?? 10);
    }

    [Test]
    public async Task Handle_Account_Pledges_Pulled()
    {
        var firstAccount = await DbContext.EmployerAccounts.FirstAsync();

        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = firstAccount.Id,
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        var actualPledges = result.Items.ToArray();

        // Assert
        var expectedPledgeRecords = await DbContext.Pledges.Where(x => x.EmployerAccount.Id == firstAccount.Id).ToListAsync();

        actualPledges.Length.Should().Be(expectedPledgeRecords.Count);
    }

    [TestCaseSource(nameof(SectorLists))]
    public async Task Handle_Pledges_Are_Filtered_By_Sector(List<Sector> sector)
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            Sectors = sector
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Sectors.Any(sector.Contains).Should().BeTrue();
        }
    }

    [TestCase(PledgeStatus.Active)]
    [TestCase(PledgeStatus.Closed)]
    public async Task Handle_Pledges_Are_Filtered_By_Status(PledgeStatus pledgeStatusFilter)
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            PledgeStatusFilter = pledgeStatusFilter
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        // Assert
        result.Items.All(x => x.Status == pledgeStatusFilter).Should().BeTrue();
    }

    [Test]
    public async Task Handle_Pledges_Are_Sorted_By_ValueLowToHigh()
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.ValueLowToHigh
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        var sortedExpectedPledges = DbContext.Pledges.OrderBy(x => x.RemainingAmount).ToList();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Id.Should().Be(sortedExpectedPledges[index].Id);
        }
    }

    [Test]
    public async Task Handle_Pledges_Are_Sorted_By_ValueHighToLow()
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.ValueHighToLow
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        var sortedExpectedPledges = DbContext.Pledges.OrderByDescending(x => x.RemainingAmount).ToList();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Id.Should().Be(sortedExpectedPledges[index].Id);
        }
    }

    [Test]
    public async Task Handle_Pledges_Are_Sorted_By_MostRecent()
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.MostRecent
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        var sortedExpectedPledges = DbContext.Pledges.OrderByDescending(x => x.CreatedOn).ToList();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Id.Should().Be(sortedExpectedPledges[index].Id);
        }
    }

    [Test]
    public async Task Handle_Pledges_Are_Sorted_By_AtoZ()
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.AtoZ
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        var sortedExpectedPledges = DbContext.Pledges.OrderBy(x => x.IsNamePublic ? x.EmployerAccount.Name : "Opportunity").ToList();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Id.Should().Be(sortedExpectedPledges[index].Id);
        }
    }

    [Test]
    public async Task Handle_Pledges_Are_Sorted_By_ZtoA()
    {
        // Arrange
        var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
        var getPledgesQuery = new GetPledgesQuery
        {
            AccountId = null,
            SortBy = OpportunitiesSortBy.ZtoA
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
        var actualPledges = result.Items.ToArray();

        // Assert
        var sortedExpectedPledges = DbContext.Pledges.OrderByDescending(x => x.IsNamePublic ? x.EmployerAccount.Name : "Opportunity").ToList();

        for (var index = 0; index < actualPledges.Length; index++)
        {
            actualPledges[index].Id.Should().Be(sortedExpectedPledges[index].Id);
        }
    }
}