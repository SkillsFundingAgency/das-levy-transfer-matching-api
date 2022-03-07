using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetPledges
{
    [TestFixture]
    public class GetPledgesQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;
        private static readonly object[] _sectorLists =
            {
                new object[] {new List<Sector> {Sector.Agriculture}},
                new object[] {new List<Sector> {Sector.Business, Sector.Charity}},
                new object[] {new List<Sector> {Sector.Education, Sector.Digital, Sector.Construction, Sector.Legal}},
                new object[] {new List<Sector> {Sector.Health, Sector.ProtectiveServices}},
                new object[] {new List<Sector> {Sector.Sales, Sector.Transport, Sector.CareServices, Sector.Catering}}
            };

        [SetUp]
        public async Task Setup()
        {
            _fixture = new Fixture();

            var employerAccounts = _fixture.CreateMany<EmployerAccount>(10).ToArray();

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

        [TestCase(null, 1)]
        [TestCase(10, 1)]
        [TestCase(5, 2)]
        [TestCase(3, 4)]
        public async Task Handle_Paging_Options_Are_Reflected_In_Results(int? pageSize, int expectedPages)
        {
            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);

            var getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = null,
                PageSize = pageSize
            };

            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);

            Assert.AreEqual(10, result.TotalItems);
            Assert.AreEqual(expectedPages, result.TotalPages);
            Assert.LessOrEqual(result.Items.Count, pageSize ?? int.MaxValue);
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

        [TestCaseSource("_sectorLists")]
        public async Task Handle_Pledges_Are_Filtered_By_Sector(List<Sector> sector)
        {
            // Arrange
            var getPledgesQueryHandler = new GetPledgesQueryHandler(DbContext);
            var getPledgesQuery = new GetPledgesQuery()
            {
                AccountId = null,
                Sectors = sector
            };

            // Act
            var result = await getPledgesQueryHandler.Handle(getPledgesQuery, CancellationToken.None);
            var actualPledges = result.Items.ToArray();

            // Assert
            for (int i = 0; i < actualPledges.Length; i++)
            {
                Assert.IsTrue(actualPledges[i].Sectors.Any(x => sector.Contains(x)));
            }
        }

    }
}