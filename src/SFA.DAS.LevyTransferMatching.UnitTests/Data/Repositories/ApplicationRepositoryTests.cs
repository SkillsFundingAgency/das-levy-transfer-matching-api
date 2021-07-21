using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Data.Repositories
{
    [TestFixture]
    public class ApplicationRepositoryTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();
        private ApplicationRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new ApplicationRepository(DbContext);
        }

        [Test]
        public async Task Add_Persists_Application()
        {
            var account = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await _repository.Add(account);
            var inserted = await DbContext.Applications.FindAsync(account.Id);
            Assert.AreEqual(account, inserted);
        }
    }
}
