using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.CreateAccount;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AccountsControllerTests
    {
        private Fixture _fixture;
        private Mock<IMediator> _mediator;
        private AccountsController _controller;
        private CreateAccountCommandResult _commandResult;
        private CreateAccountRequest _apiRequest;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mediator = new Mock<IMediator>();
            _controller = new AccountsController(_mediator.Object);
            
            _commandResult = new CreateAccountCommandResult {Created = true};
            _apiRequest = _fixture.Create<CreateAccountRequest>();

            _mediator
                .Setup(x => x.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_commandResult);
        }

        [Test]
        public async Task POST_Create_Returns_CreatedResult()
        {
            _commandResult.Created = true;

            var actionResult = await _controller.CreateAccount(_apiRequest);

            var createdResult = actionResult as CreatedResult;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, (int)HttpStatusCode.Created);
            Assert.AreEqual(createdResult.Location, $"/accounts/{_apiRequest.AccountId}");
        }

        [Test]
        public async Task POST_Create_Returns_OkResult()
        {
            _commandResult.Created = false;

            var actionResult = await _controller.CreateAccount(_apiRequest);

            var okResult = actionResult as OkResult;
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, (int)HttpStatusCode.OK);
        }
    }
}
