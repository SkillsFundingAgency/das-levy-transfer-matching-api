using AutoFixture;
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
        private Mock<IPledgesDataRepository> _mockPledgesDataRepository;
        private CreatePledgeHandler _createPledgeHandler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockHashingService = new Mock<IHashingService>();
            _mockPledgesDataRepository = new Mock<IPledgesDataRepository>();
            _createPledgeHandler = new CreatePledgeHandler(_mockHashingService.Object, _mockPledgesDataRepository.Object);
        }

        [Test]
        public async Task Handle_Pledge_Created_Id_Returned()
        {
            // Arrange
            var pledge = _fixture.Create<Pledge>();
            var command = new CreatePledgeCommand()
            {
                Pledge = pledge,
            };
            var decodedAccountId = _fixture.Create<int>();
            var expectedId = _fixture.Create<int>();

            _mockHashingService
                .Setup(x => x.DecodeValue(It.Is<string>(x => x == pledge.EncodedAccountId)))
                .Returns(decodedAccountId);

            bool addedToDatabase = false;
            _mockPledgesDataRepository
                .Setup(x => x.Add(It.Is<Pledge>(x => x == pledge)))
                .Callback(() =>
                {
                    addedToDatabase = true;

                    pledge.Id = expectedId;
                });

            // Act
            var result = await _createPledgeHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pledge.AccountId, decodedAccountId);
            Assert.IsTrue(addedToDatabase);
            Assert.AreEqual(result.Id, expectedId);
        }
    }
}