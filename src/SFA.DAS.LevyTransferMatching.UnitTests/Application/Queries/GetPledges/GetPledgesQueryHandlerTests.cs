using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.Enums;
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
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_All_Pledges_Pulled_And_Stitched_Up_With_Accounts()
        {
            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);

            var pledges = new List<Pledge>();

            for (var i = 0; i < employerAccounts.Count(); i++)
            {
                pledges.Add(
                    employerAccounts[i].CreatePledge(
                        _fixture.Create<int>(),
                        _fixture.Create<bool>(),
                        _fixture.Create<Level>(),
                        _fixture.Create<JobRole>(),
                        _fixture.Create<Sector>(),
                        _fixture.Create<List<PledgeLocation>>(),
                        _fixture.Create<UserInfo>()
                    ));
            }
            
            await DbContext.Pledges.AddRangeAsync(pledges);

            await DbContext.SaveChangesAsync();

            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery();

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            // Assert
            var dbPledges = await DbContext.Pledges.OrderByDescending(x => x.Amount).ToArrayAsync();

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(result[i].Id, dbPledges[i].Id);
                Assert.AreEqual(result[i].AccountId, dbPledges[i].EmployerAccount.Id);
            }
        }
    }
}