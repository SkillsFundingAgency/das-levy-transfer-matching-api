using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using SFA.DAS.LevyTransferMatching.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledge
{
    public class GetPledgeQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();

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
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DasAccountName);
            Assert.AreEqual(expectedPledge.Id, result.Id);
        }

        [Test]
        public async Task Handle_Individual_Pledge_Not_Found_Null_Returned()
        {
            await PopulateDbContext();

            var getPledgesQueryHandler = new GetPledgeQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgeQuery()
            {
                Id = -1,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }

        protected async Task PopulateDbContext()
        {
            EmployerAccount[] employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            Pledge[] pledges = _fixture.CreateMany<Pledge>().ToArray();

            for (int i = 0; i < pledges.Length; i++)
            {
                pledges[i].EmployerAccount = employerAccounts[i];
            }

            await DbContext.Pledges.AddRangeAsync(pledges);

            await DbContext.SaveChangesAsync();
        }
    }
}