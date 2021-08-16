﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Api.Models.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;

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
                    command.BusinessWebsite == _request.BusinessWebsite &&
                    command.UserId == _request.UserId &&
                    command.UserDisplayName == _request.UserDisplayName
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

        [Test]
        public async Task Get_Returns_Application()
        {
            // Arrange
            var pledgeId = _fixture.Create<int>();
            var applicationId = _fixture.Create<int>();
            var applicationResult = _fixture.Create<GetApplicationResult>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => y.Id == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applicationResult);

            // Act
            var actionResult = await _applicationsController.GetApplication(pledgeId, applicationId);
            var okObjectResult = actionResult as OkObjectResult;
            var getApplicationResponse = okObjectResult.Value as GetApplicationResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getApplicationResponse);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_Returns_NotFound()
        {
            // Arrange
            var pledgeId = _fixture.Create<int>();
            var applicationId = _fixture.Create<int>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => y.Id == applicationId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationResult)null);

            // Act
            var actionResult = await _applicationsController.GetApplication(pledgeId, applicationId);
            var okObjectResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.NotFound);
        }
    }
}
