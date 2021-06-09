using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.HashingService;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    [TestFixture]
    public class CreatePledgeHandlerTests
    {
        private Fixture _fixture;
        private Mock<IHashingService> _mockHashingService;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockHashingService = new Mock<IHashingService>();
        }

        [Test]
        public async Task Handle_Pledge_Created_Id_Returned()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase("SFA.DAS.LevyTransferMatching.Database")
                .Options;

            var dbContext = new LevyTransferMatchingDbContext(options);
            var createPledgeHandler = new CreatePledgeHandler(dbContext, _mockHashingService.Object);

            var command = _fixture.Create<CreatePledgeCommand>();
            var decodedAccountId = _fixture.Create<int>();
            var expectedId = 1;

            _mockHashingService
                .Setup(x => x.DecodeValue(It.Is<string>(x => x == command.EncodedAccountId)))
                .Returns(decodedAccountId);

            // Act
            var result = await createPledgeHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(command.AccountId, decodedAccountId);
            Assert.AreEqual(result.Id, expectedId);
        }
    }
}