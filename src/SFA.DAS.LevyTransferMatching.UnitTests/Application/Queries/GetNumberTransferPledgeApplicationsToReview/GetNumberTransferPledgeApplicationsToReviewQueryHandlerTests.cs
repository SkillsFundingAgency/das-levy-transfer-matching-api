using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetNumberTransferPledgeApplicationsToReview;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetNumberTransferPledgeApplicationsToReview
{
    public class GetNumberTransferPledgeApplicationsToReviewQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private readonly long _employerAccountId = 123;
        private Fixture _fixture;
        private GetNumberTransferPledgeApplicationsToReviewQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _handler = new GetNumberTransferPledgeApplicationsToReviewQueryHandler(DbContext);
        }

        [Test]
        public async Task Handle_Where_No_Pending_Applications_Exist()
        {
            await PopulateDbContext();

            // Act
            var result = await _handler.Handle(new GetNumberTransferPledgeApplicationsToReviewQuery { TransferSenderId = _employerAccountId }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.NumberTransferPledgeApplicationsToReview);
            Assert.AreEqual(0, result.NumberTransferPledgeApplicationsToReview);
        }

        [Test]
        public async Task Handle_Where_Pending_Applications_Exist()
        {
            await PopulateDbContext();
            await InsertPendingApplication_into_DbContext();

            // Act
            var result = await _handler.Handle(new GetNumberTransferPledgeApplicationsToReviewQuery { TransferSenderId = _employerAccountId }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.NumberTransferPledgeApplicationsToReview);
            Assert.AreEqual(1, result.NumberTransferPledgeApplicationsToReview);
        }

        protected async Task PopulateDbContext()
        {
            var employerAccounts = _fixture.CreateMany<EmployerAccount>().ToArray();

            var pledges = new List<Pledge>();
            employerAccounts[0].SetValue(x => x.Id, _employerAccountId);

            foreach (var account in employerAccounts)
            {
                var pledge = account.CreatePledge(
                      _fixture.Create<CreatePledgeProperties>(),
                      _fixture.Create<UserInfo>()
                  );

                pledge.SetValue(x => x.Status, PledgeStatus.Active);
                pledges.Add(pledge);
            }

            var applications = new List<Data.Models.Application>();
            foreach (var pledge in pledges)
            {
                var app = pledge.CreateApplication(pledge.EmployerAccount, new CreateApplicationProperties(), new UserInfo("", ""));
                app.SetValue(x => x.Status, ApplicationStatus.Approved);
                applications.Add(app);
            }
            await DbContext.EmployerAccounts.AddRangeAsync(employerAccounts);
            await DbContext.Pledges.AddRangeAsync(pledges);
            await DbContext.Applications.AddRangeAsync(applications);

            await DbContext.SaveChangesAsync();
        }

        protected async Task InsertPendingApplication_into_DbContext()
        {
            var pledges = await DbContext.Pledges.Where(x => x.EmployerAccountId == _employerAccountId).ToListAsync();

            var app = pledges[0].CreateApplication(pledges[0].EmployerAccount, new CreateApplicationProperties(), new UserInfo("", ""));
            app.SetValue(x => x.Status, ApplicationStatus.Pending);

            await DbContext.Applications.AddAsync(app);

            await DbContext.SaveChangesAsync();
        }
    }
}