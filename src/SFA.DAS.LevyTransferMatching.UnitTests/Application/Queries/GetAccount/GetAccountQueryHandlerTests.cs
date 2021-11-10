using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetAccount
{
    [TestFixture]
    public class GetAccountQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;
        private EmployerAccount _account;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _account = _fixture.Create<EmployerAccount>();
            ResetDbContext();
            DbContext.EmployerAccounts.Add(_account);
            DbContext.SaveChanges();
        }

        [Test]
        public async Task Handle_Account_Is_Returned()
        {
            var handler = new GetAccountQueryHandler(DbContext);
            var query = new GetAccountQuery{ AccountId = _account.Id };

            var result =  await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(_account.Id, result.AccountId);
            Assert.AreEqual(_account.Name, result.AccountName);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Account_Is_Not_Found()
        {
            var handler = new GetAccountQueryHandler(DbContext);
            var query = new GetAccountQuery { AccountId = _account.Id + 1 };

            var result = await handler.Handle(query, CancellationToken.None);
            Assert.IsNull(result);
        }
    }
}
