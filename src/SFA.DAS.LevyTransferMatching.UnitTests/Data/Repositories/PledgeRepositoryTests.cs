using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Services.Events;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Data.Repositories
{
    [TestFixture]
    public class PledgeRepositoryTests : LevyTransferMatchingDbContextFixture
    {
        private readonly Fixture _fixture = new Fixture();
        private PledgeRepository _repository;
        private Mock<IDomainEventDispatcher> _domainEventDispatcher;

        [SetUp]
        public void SetUp()
        {
            _domainEventDispatcher = new Mock<IDomainEventDispatcher>();
            _domainEventDispatcher.Setup(x => x.Send(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _repository = new PledgeRepository(DbContext, _domainEventDispatcher.Object);
        }

        [Test]
        public async Task Get_Retrieves_Pledge()
        {
            var pledge = _fixture.Create<Pledge>();
            DbContext.Pledges.Add(pledge);
            DbContext.SaveChanges();

            var result = await _repository.Get(pledge.Id);

            Assert.AreEqual(pledge, result);
        }

        [Test]
        public async Task Add_Persists_Pledge()
        {
            var pledge = _fixture.Create<Pledge>();
            await _repository.Add(pledge);
            var inserted = await DbContext.Pledges.FindAsync(pledge.Id);
            Assert.AreEqual(pledge, inserted);
        }
    }
}