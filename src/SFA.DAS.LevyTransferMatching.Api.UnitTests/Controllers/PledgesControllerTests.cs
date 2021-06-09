using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers
{
    [TestFixture]
    public class PledgesControllerTests
    {
        private Fixture _fixture;
        private Mock<IMediator> _mockMediator;
        private PledgesController _pledgesController;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockMediator = new Mock<IMediator>();
            _pledgesController = new PledgesController(_mockMediator.Object);
        }

        [Test]
        public async Task POST_Create_Returns_Created_With_Correct_Location()
        {
            // Arrange 
            var encodedAccountId = _fixture.Create<string>();
            var request = _fixture.Create<CreatePledgeRequest>();
            var result = _fixture.Create<CreatePledgeResult>();

            _mockMediator
                .Setup(x => x.Send(It.IsAny<CreatePledgeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _pledgesController.Create(encodedAccountId, request);
            var createdResult = actionResult as CreatedResult;
            var pledgeReference = createdResult.Value as CreatePledgeResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(pledgeReference);
            Assert.AreEqual(createdResult.StatusCode, (int)HttpStatusCode.Created);
            Assert.AreEqual(createdResult.Location, $"/accounts/{encodedAccountId}/pledges/{result.Id}");
            Assert.AreEqual(pledgeReference.Id, result.Id);
        }
    }
}