using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests : LevyTransferMatchingDbContextFixture
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
            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            var pledgeRecords = _fixture.CreateMany<Pledge>().ToArray();

            for (var i = 0; i < pledgeRecords.Length; i++)
            {
                pledgeRecords[i].EmployerAccount = employerAccounts[i];
            }
            
            await DbContext.Pledges.AddRangeAsync(pledgeRecords);

            await DbContext.SaveChangesAsync();

            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery();

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            var pledges = result.Pledges.ToArray();

            // Assert
            pledgeRecords = await DbContext.Pledges.OrderByDescending(x => x.Amount).ToArrayAsync();

            for (int i = 0; i < pledges.Length; i++)
            {
                Assert.AreEqual(pledges[i].Id, pledgeRecords[i].Id);
                Assert.AreEqual(pledges[i].AccountId, pledgeRecords[i].EmployerAccount.Id);
            }
        }
    }
}