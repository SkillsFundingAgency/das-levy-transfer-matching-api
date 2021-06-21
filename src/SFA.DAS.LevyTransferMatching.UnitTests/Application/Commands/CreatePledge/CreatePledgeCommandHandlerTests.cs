using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    [TestFixture]
    public class CreatePledgeCommandHandlerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Pledge_Created_Id_Returned_And_Flags_Stored_Correctly_And_EmployerAccount_Inserted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);
            var createPledgeHandler = new CreatePledgeCommandHandler(dbContext);
            var command = _fixture.Create<CreatePledgeCommand>();

            var expectedId = 1;

            // Act
            var result = await createPledgeHandler.Handle(command, CancellationToken.None);

            var insertedPledge = dbContext.Pledges.Find(result.Id);

            var storedJobRoles = insertedPledge.JobRoles.GetFlags<JobRole>();
            var storedLevels = insertedPledge.JobRoles.GetFlags<Level>();
            var storedSectors = insertedPledge.Sectors.GetFlags<Sector>();

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(result.Id, expectedId);

            CollectionAssert.AreEqual(command.JobRoles, storedJobRoles);
            CollectionAssert.AreEqual(command.Levels, storedLevels);
            CollectionAssert.AreEqual(command.Sectors, storedSectors);

            var employerAccount = await dbContext.EmployerAccounts.ToListAsync();
            Assert.AreEqual(employerAccount.Count, 1);
        }
    }
}