using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

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

            var pledges = new List<Pledge>();

            for (var i = 0; i < employerAccounts.Count(); i++)
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

            var getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = null,
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            var actualPledges = result.Items.ToArray();

            // Assert
            var dbPledges = await DbContext.Pledges.OrderByDescending(x => x.Amount).ToArrayAsync();

            for (int i = 0; i < actualPledges.Length; i++)
            {
                Assert.AreEqual(actualPledges[i].Id, dbPledges[i].Id);
                Assert.AreEqual(actualPledges[i].AccountId, dbPledges[i].EmployerAccount.Id);
            }
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