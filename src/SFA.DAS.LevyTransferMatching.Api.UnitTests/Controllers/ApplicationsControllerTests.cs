using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ApplicationsControllerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IMediator> _mediator;
        private ApplicationsController _applicationsController;

        private long _accountId;
        private int _pledgeId;
        private CreateApplicationRequest _request;
        private CreateApplicationCommandResult _result;

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<long>();
            _pledgeId = _fixture.Create<int>();
            _request = _fixture.Create<CreateApplicationRequest>();
            _result = _fixture.Create<CreateApplicationCommandResult>();

            _mediator = new Mock<IMediator>();
            _applicationsController = new ApplicationsController(_mediator.Object);

            _mediator.Setup(x => x.Send(It.Is<CreateApplicationCommand>(command =>
                    command.PledgeId == _pledgeId && command.EmployerAccountId == _accountId &&
                    command.ReceiverEmployerAccountId == _request.ReceiverEmployerAccountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_result);
        }

        [Test]
        public async Task Post_Returns_ApplicationId()
        {
            var actionResult = await _applicationsController.CreateApplication(_accountId, _pledgeId, _request);
            var createdResult = actionResult as CreatedResult;
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as CreateApplicationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.ApplicationId, response.ApplicationId);
        }
    }
}
