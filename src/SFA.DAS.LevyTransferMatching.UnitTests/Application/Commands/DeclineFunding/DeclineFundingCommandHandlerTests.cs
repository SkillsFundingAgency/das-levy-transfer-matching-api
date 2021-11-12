using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DeclineFunding
{
    public class DeclineFundingCommandHandlerTests
    {
        private Fixture _fixture;
        private Mock<IApplicationRepository> _mockApplicationRepository;
        private DeclineFundingCommandHandler _declineFundingCommandHandler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _mockApplicationRepository = new Mock<IApplicationRepository>();
            var mockLogger = new Mock<ILogger<DeclineFundingCommandHandler>>();

            _declineFundingCommandHandler = new DeclineFundingCommandHandler(_mockApplicationRepository.Object, mockLogger.Object);
        }

        [Test]
        public async Task Handle_ApplicationExists_ApplicationUpdatedToDeclined()
        {
            // Arrange
            var request = _fixture.Create<DeclineFundingCommand>();

            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();

            var userInfo = _fixture.Create<UserInfo>();

            // Make it 'approved', otherwise declining will fail
            application.Approve(userInfo);

            _mockApplicationRepository
                .Setup(x => x.Get(It.Is<int>(y => y == request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == request.AccountId)))
                .ReturnsAsync(application);

            LevyTransferMatching.Data.Models.Application updatedApplication = null;
            Action<LevyTransferMatching.Data.Models.Application> updateCallback =
                (x) =>
                {
                    updatedApplication = x;
                };

            _mockApplicationRepository
                .Setup(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(y => y == application)))
                .Callback(updateCallback);

            // Act
            var result = await _declineFundingCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.Updated);
            Assert.IsNotNull(updatedApplication);
            Assert.AreEqual(ApplicationStatus.Declined, updatedApplication.Status);
        }

        [Test]
        public async Task Handle_ApplicationDoesntExist_ApplicationNotUpdated()
        {
            // Arrange
            var request = _fixture.Create<DeclineFundingCommand>();

            _mockApplicationRepository
                .Setup(x => x.Get(It.Is<int>(y => y == request.ApplicationId), It.Is<int?>(y => y == null), It.Is<long?>(y => y == request.AccountId)))
                .ReturnsAsync((LevyTransferMatching.Data.Models.Application)null);

            // Act
            var result = await _declineFundingCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.Updated);
        }
    }
}