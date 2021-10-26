﻿using System.Threading;
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
        public async Task Update_Persists_Application()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.Applications.AddAsync(application, CancellationToken.None);
            await DbContext.SaveChangesAsync();

            application.Approve(_fixture.Create<UserInfo>());
            await _repository.Update(application);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            var updated = await DbContext.Applications.FindAsync(application.Id);

            Assert.AreEqual(ApplicationStatus.Approved, updated.Status);
        }

        [Test]
        public async Task Reject_Persists_Application()
        {
            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.Applications.AddAsync(application, CancellationToken.None);
            await DbContext.SaveChangesAsync();

            application.Reject(_fixture.Create<UserInfo>());
            await _repository.Update(application);
            await DbContext.SaveChangesAsync(CancellationToken.None);

            var updated = await DbContext.Applications.FindAsync(application.Id);

            Assert.AreEqual(ApplicationStatus.Rejected, updated.Status);
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

            var result = await _repository.Get(null, application.Pledge.Id, application.Id);

            Assert.AreEqual(application, result);
        }
    }
}
