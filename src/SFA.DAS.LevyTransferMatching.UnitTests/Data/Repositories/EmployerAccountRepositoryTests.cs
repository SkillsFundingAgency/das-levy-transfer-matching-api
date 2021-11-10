using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Data.Repositories
{
    [TestFixture]
    public class EmployerAccountRepositoryTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();
        private EmployerAccountRepository _repository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _repository = new EmployerAccountRepository(DbContext);
        }

        [Test]
        public async Task Get_Retrieves_EmployerAccount()
        {
            var account = _fixture.Create<EmployerAccount>();
            DbContext.EmployerAccounts.Add(account);
            DbContext.SaveChanges();

            var result = await _repository.Get(account.Id);

            Assert.AreEqual(account, result);
        }

        [Test]
        public async Task Add_Persists_EmployerAccount()
        {
            var account = _fixture.Create<EmployerAccount>();
            await _repository.Add(account);
            var inserted = await DbContext.EmployerAccounts.FindAsync(account.Id);
            Assert.AreEqual(account, inserted);
        }
    }
}
