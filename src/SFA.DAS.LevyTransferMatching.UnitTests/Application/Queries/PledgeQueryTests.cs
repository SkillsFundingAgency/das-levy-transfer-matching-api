using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries
{
    public abstract class PledgeQueryTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        protected async Task PopulateDbContext(LevyTransferMatchingDbContext dbContext)
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