using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledge;

public class GetPledgeQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private readonly Fixture _fixture = new();

    [Test]
    public async Task Handle_Individual_Pledge_Pulled_And_Stitched_Up_With_Account()
    {
        await PopulateDbContext();

        var expectedPledge = await DbContext.Pledges.OrderByDescending(x => x.Amount).FirstAsync();

        var getPledgesQueryHandler = new GetPledgeQueryHandler(DbContext);

        var getPledgesQuery = new GetPledgeQuery()
        {
            Id = expectedPledge.Id,
        };

        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DasAccountName.Should().NotBeNull();
        Assert.That(result.Id, Is.EqualTo(expectedPledge.Id));
    }

    [Test]
    public async Task Handle_Individual_Pledge_Not_Found_Null_Returned()
    {
        await PopulateDbContext();

        var getPledgesQueryHandler = new GetPledgeQueryHandler(DbContext);

        var getPledgesQuery = new GetPledgeQuery { Id = -1, };
        
        // Act
        var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    private async Task PopulateDbContext()
    {
        var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

        await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

        var pledges = new List<Pledge>();

        foreach (var account in employerAccounts)
        {
            var pledge = account.CreatePledge(
                _fixture.Create<CreatePledgeProperties>(),
                _fixture.Create<UserInfo>()
            );
            
            pledges.Add(pledge);
        }

        await DbContext.Pledges.AddRangeAsync(pledges);

        await DbContext.SaveChangesAsync();
    }
}