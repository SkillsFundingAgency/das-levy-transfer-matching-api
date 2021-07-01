using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests : PledgeQueryTests
    {
        [Test]
        public async Task Handle_All_Pledges_Pulled_And_Stitched_Up_With_Accounts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);

            await PopulateDbContext(dbContext);

            var getPledgesQueryHandler = new GetPledgesQueryHandler(dbContext);

            var getPledgesQuery = new GetPledgesQuery();

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            var pledges = await dbContext.Pledges.ToArrayAsync();
            var employerAccounts = await dbContext.EmployerAccounts.ToArrayAsync();

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].Id, pledges[i].Id);
                Assert.AreEqual(result[i].AccountId, employerAccounts[i].Id);
            }
        }
    }
}