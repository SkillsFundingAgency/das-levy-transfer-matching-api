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
        public async Task Handle_Returns_Pledge_Details()
        {
            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            var pledges = _fixture.CreateMany<Pledge>().ToArray();

            for (var i = 0; i < pledges.Length; i++)
            {
                pledges[i].EmployerAccount = employerAccounts[i];
            }
            
            await DbContext.Pledges.AddRangeAsync(pledges);

            await DbContext.SaveChangesAsync();

            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery();

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            pledges = await DbContext.Pledges.ToArrayAsync();
            employerAccounts = await DbContext.EmployerAccounts.ToArrayAsync();

            for (var i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].Id, pledges[i].Id);
                Assert.AreEqual(result[i].AccountId, employerAccounts[i].Id);
            }
        }
    }
}