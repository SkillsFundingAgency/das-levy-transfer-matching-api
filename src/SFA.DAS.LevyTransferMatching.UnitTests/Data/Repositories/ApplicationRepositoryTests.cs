using System.Threading;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System.Threading.Tasks;
using Moq;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Data.Repositories
{
    [TestFixture]
    public class ApplicationRepositoryTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();
        private ApplicationRepository _repository;
        private Mock<IDomainEventDispatcher> _domainEventDispatcher;

        [SetUp]
        public void SetUp()
        {
            _domainEventDispatcher = new Mock<IDomainEventDispatcher>();
            _domainEventDispatcher.Setup(x => x.Send(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _repository = new ApplicationRepository(DbContext, _domainEventDispatcher.Object);
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
