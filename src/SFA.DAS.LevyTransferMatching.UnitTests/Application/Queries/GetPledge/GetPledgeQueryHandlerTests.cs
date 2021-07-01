using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledge
{
    public class GetPledgeQueryHandlerTests : PledgeQueryTests
    {
        [Test]
        public async Task Handle_Individual_Pledge_Pulled_And_Stitched_Up_With_Account()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);

            await PopulateDbContext(dbContext);

            var expectedPledge = await dbContext.Pledges.FirstAsync();

            var getPledgesQueryHandler = new GetPledgeQueryHandler(dbContext);

            var getPledgesQuery = new GetPledgeQuery()
            {
                Id = expectedPledge.Id,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DasAccountName);
            Assert.AreEqual(expectedPledge.Id, result.Id);
        }

        [Test]
        public async Task Handle_Individual_Pledge_Not_Found_Null_Returned()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);

            await PopulateDbContext(dbContext);

            var getPledgesQueryHandler = new GetPledgeQueryHandler(dbContext);

            var getPledgesQuery = new GetPledgeQuery()
            {
                Id = -1,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }
    }
}