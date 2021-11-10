using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledge
{
    public class GetPledgeQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

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
    }
}