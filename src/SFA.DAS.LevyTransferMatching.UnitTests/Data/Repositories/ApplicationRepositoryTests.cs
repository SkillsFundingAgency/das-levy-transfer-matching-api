using System.Threading;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;
using System.Threading.Tasks;
using Moq;
using SFA.DAS.LevyTransferMatching.Abstractions.Events;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Testing;

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
            ResetDbContext();
            _repository = new ApplicationRepository(DbContext, _domainEventDispatcher.Object);
        }

        [Test]
        public async Task Add_Persists_Application()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await _repository.Add(application);
            var inserted = await DbContext.Applications.FindAsync(application.Id);
            Assert.AreEqual(application, inserted);
        }

        [Test]
        public async Task Update_Persists_Application_With_Approve_ApplicationStatus_Approved()
        {
            var application = await CreateApprovedApplication();

            Assert.AreEqual(ApplicationStatus.Approved, application.Status);
        }

        [Test]
        public async Task Update_Persists_Application_With_AcceptFunding_ApplicationStatus_Accepted()
        {
            var application = await CreateApprovedApplication();

            application.AcceptFunding(_fixture.Create<UserInfo>());
            await _repository.Update(application);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            var updated = await DbContext.Applications.FindAsync(application.Id);

            Assert.AreEqual(ApplicationStatus.Accepted, updated.Status);
        }

        [Test]
        public async Task Get_Retrieves_Application()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            var empAccount = _fixture.Create<EmployerAccount>();
            empAccount.SetValue(o => o.Id, 1); 
            application.SetValue(o => o.EmployerAccount, empAccount);

            DbContext.Applications.Add(application);
            DbContext.SaveChanges();

            var result = await _repository.Get(application.Pledge.Id, null, application.Id);

            Assert.AreEqual(application, result);
        }

        [Test]
        public async Task Update_Persists_Application_With_Withdraw_ApplicationStatus_Withdrawn()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.Applications.AddAsync(application, CancellationToken.None);

            application.Withdraw(_fixture.Create<UserInfo>());
            await _repository.Update(application);

            var updated = await DbContext.Applications.FindAsync(application.Id);

            Assert.AreEqual(ApplicationStatus.Withdrawn, updated.Status);
        }

        private async Task<LevyTransferMatching.Data.Models.Application> CreateApprovedApplication()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.Applications.AddAsync(application, CancellationToken.None);
            await DbContext.SaveChangesAsync();

            application.Approve(_fixture.Create<UserInfo>());
            await _repository.Update(application);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            var updated = await DbContext.Applications.FindAsync(application.Id);

            return updated;
        }
    }
}
