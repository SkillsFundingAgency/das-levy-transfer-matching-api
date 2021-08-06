using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;

        [SetUp]
        public async Task Setup()
        {
            _fixture = new Fixture();

            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            var pledgeRecords = _fixture.CreateMany<Pledge>().ToArray();

            for (var i = 0; i < pledgeRecords.Length; i++)
            {
                pledgeRecords[i].EmployerAccount = employerAccounts[i];
            }

            await DbContext.Pledges.AddRangeAsync(pledgeRecords);

            await DbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Handle_All_Pledges_Pulled_And_Stitched_Up_With_Accounts()
        {
            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = null,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            var actualPledges = result.Items.ToArray();

            // Assert
            var expectedPledgeRecords = await DbContext.Pledges.OrderByDescending(x => x.Amount).ToArrayAsync();

            for (int i = 0; i < actualPledges.Length; i++)
            {
                Assert.AreEqual(expectedPledgeRecords[i].Id, actualPledges[i].Id);
                Assert.AreEqual(expectedPledgeRecords[i].EmployerAccount.Id, actualPledges[i].AccountId);
            }

            Assert.AreEqual(result.TotalItems, actualPledges.Length);
        }

        [Test]
        public async Task Handle_Account_Pledges_Pulled()
        {
            var firstAccount = await DbContext.EmployerAccounts.FirstAsync();

            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = firstAccount.Id,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            var actualPledges = result.Items.ToArray();

            // Assert
            var expectedPledgeRecords = await DbContext.Pledges.Where(x => x.EmployerAccount.Id == firstAccount.Id).ToListAsync();

            Assert.AreEqual(expectedPledgeRecords.Count(), actualPledges.Count());
        }
    }
}