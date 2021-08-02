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

        private int _pledgeId;
        private CreateApplicationRequest _request;
        private CreateApplicationCommandResult _result;

        [SetUp]
        public void Setup()
        {
            _pledgeId = _fixture.Create<int>();
            _request = _fixture.Create<CreateApplicationRequest>();
            _result = _fixture.Create<CreateApplicationCommandResult>();

            _mediator = new Mock<IMediator>();
            _applicationsController = new ApplicationsController(_mediator.Object);

            _mediator.Setup(x => x.Send(It.Is<CreateApplicationCommand>(command =>
                    command.PledgeId == _pledgeId &&
                    command.EmployerAccountId == _request.EmployerAccountId &&
                    command.Details == _request.Details && 
                    command.StandardId == _request.StandardId &&
                    command.NumberOfApprentices == _request.NumberOfApprentices &&
                    command.StartDate == _request.StartDate &&
                    command.HasTrainingProvider == _request.HasTrainingProvider &&
                    command.Amount == _request.Amount &&
                    command.Sectors.Equals(_request.Sectors) &&
                    command.Postcode == _request.Postcode &&
                    command.FirstName == _request.FirstName &&
                    command.LastName == _request.LastName &&
                    command.EmailAddresses.Equals(_request.EmailAddresses) &&
                    command.BusinessWebsite == _request.BusinessWebsite
                    ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_result);
        }

        [Test]
        public async Task Post_Returns_ApplicationId()
        {
            var actionResult = await _applicationsController.CreateApplication(_pledgeId, _request);
            var createdResult = actionResult as CreatedResult;
            Assert.IsNotNull(createdResult);
            var response = createdResult.Value as CreateApplicationResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(_result.ApplicationId, response.ApplicationId);
        }
    }
}
