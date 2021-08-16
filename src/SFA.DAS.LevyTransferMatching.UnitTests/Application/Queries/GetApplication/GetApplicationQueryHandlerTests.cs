using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandlerTests
    {
        private Fixture _fixture;
        private Mock<IApplicationRepository> _mockApplicationRepository;
        private GetApplicationQueryHandler _getApplicationQueryHandler;

        [SetUp]
        public void Arrange()
        {
            _fixture = new Fixture();

            _mockApplicationRepository = new Mock<IApplicationRepository>();

            _getApplicationQueryHandler = new GetApplicationQueryHandler(_mockApplicationRepository.Object);
        }

        [Test]
        public async Task Handle_Application_Found_Returns_Result()
        {
            // Arrange
            var applicationId = _fixture.Create<int>();
            var request = new GetApplicationQuery()
            {
                Id = applicationId,
            };
            var application = new LevyTransferMatching.Data.Models.Application(
                _fixture.Create<Pledge>(),
                _fixture.Create<EmployerAccount>(),
                _fixture.Create<CreateApplicationProperties>(),
                _fixture.Create<UserInfo>());

            _mockApplicationRepository.Setup(x => x.Get(It.Is<int>(y => y == applicationId))).ReturnsAsync(application);

            // Act
            var result = await _getApplicationQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Handle_Application_Not_Found_Returns_Null()
        {
            // Arrange
            var applicationId = _fixture.Create<int>();
            var request = new GetApplicationQuery()
            {
                Id = applicationId,
            };
            LevyTransferMatching.Data.Models.Application application = null;

            _mockApplicationRepository.Setup(x => x.Get(It.Is<int>(y => y == applicationId))).ReturnsAsync(application);

            // Act
            var result = await _getApplicationQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }
    }
}