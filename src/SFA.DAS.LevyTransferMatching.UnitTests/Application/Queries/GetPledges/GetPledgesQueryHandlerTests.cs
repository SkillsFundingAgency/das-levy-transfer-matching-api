using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

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

            var getPledgesQueryHandler = new GetPledgesQueryHandler(dbContext);

            var getPledgesQuery = new GetPledgesQuery()
            {
                Id = expectedPledge.Id,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            var actualPledge = result.SingleOrDefault();

            Assert.IsNotNull(actualPledge);
            Assert.IsNotNull(actualPledge.DasAccountName);
            Assert.AreEqual(expectedPledge.Id, actualPledge.Id);
        }

        private async Task PopulateDbContext(LevyTransferMatchingDbContext dbContext)
        {
            EmployerAccount[] employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await dbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            Pledge[] pledges = _fixture.CreateMany<Pledge>().ToArray();

            for (int i = 0; i < pledges.Length; i++)
            {
                pledges[i].EmployerAccount = employerAccounts[i];
            }

            await dbContext.Pledges.AddRangeAsync(pledges);

            await dbContext.SaveChangesAsync();
        }
    }
}