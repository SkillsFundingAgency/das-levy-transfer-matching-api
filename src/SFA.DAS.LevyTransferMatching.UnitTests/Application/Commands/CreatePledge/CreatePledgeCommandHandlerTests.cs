using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    [TestFixture]
    public class CreatePledgeCommandHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Pledge_Created_Id_Returned_And_Flags_Stored_Correctly()
        {
            var createPledgeHandler = new CreatePledgeCommandHandler(DbContext);
            var command = _fixture.Create<CreatePledgeCommand>();

            var expectedId = 1;

            // Act
            var result = await createPledgeHandler.Handle(command, CancellationToken.None);

            var insertedPledge = DbContext.Pledges.Find(result.Id);

            var storedJobRoles = insertedPledge.JobRoles.GetFlags<JobRole>();
            var storedLevels = insertedPledge.JobRoles.GetFlags<Level>();
            var storedSectors = insertedPledge.Sectors.GetFlags<Sector>();

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(result.Id, expectedId);

            CollectionAssert.AreEqual(command.JobRoles, storedJobRoles);
            CollectionAssert.AreEqual(command.Levels, storedLevels);
            CollectionAssert.AreEqual(command.Sectors, storedSectors);
        }
    }
}