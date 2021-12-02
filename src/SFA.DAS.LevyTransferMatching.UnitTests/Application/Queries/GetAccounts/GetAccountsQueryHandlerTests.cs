using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccounts;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetAccounts
{
    [TestFixture]
    public class GetAccountsQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;
        private List<EmployerAccount> _accounts;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _accounts = _fixture.Create<List<EmployerAccount>>();
            DbContext.EmployerAccounts.AddRange(_accounts);
            DbContext.SaveChanges();
        }

        [Test]
        public async Task Handle_Accounts_Are_Returned()
        {
            var handler = new GetAccountsQueryHandler(DbContext);
            var query = new GetAccountsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(_accounts.Count, result.EmployerAccounts.Count);
            Assert.AreEqual(_accounts[0].Id, result.EmployerAccounts[0].Id);
        }
    }
}
