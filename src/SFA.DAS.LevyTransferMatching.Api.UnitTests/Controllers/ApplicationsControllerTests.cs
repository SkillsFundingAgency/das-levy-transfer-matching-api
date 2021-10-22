using System.Net;
using System.Linq;
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
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ApplicationsControllerTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IMediator> _mediator;
        private ApplicationsController _applicationsController;

        private int _pledgeId;
        private int _applicationId;
        private long _accountId;
        private CreateApplicationRequest _request;
        private CreateApplicationCommandResult _result;
        private DebitApplicationRequest _debitApplicationRequest;

        [SetUp]
        public void Setup()
        {
            _pledgeId = _fixture.Create<int>();
            _applicationId = _fixture.Create<int>();
            _accountId = _fixture.Create<long>();
            _request = _fixture.Create<CreateApplicationRequest>();
            _result = _fixture.Create<CreateApplicationCommandResult>();
            _debitApplicationRequest = _fixture.Create<DebitApplicationRequest>();

            _mediator = new Mock<IMediator>();
            _applicationsController = new ApplicationsController(_mediator.Object);

            _mediator.Setup(x => x.Send(It.IsAny<ApproveApplicationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Unit.Value);

            _mediator.Setup(x => x.Send(It.Is<DebitApplicationCommand>(command =>
                    command.ApplicationId == _applicationId &&
                    command.Amount == _debitApplicationRequest.Amount &&
                    command.NumberOfApprentices == _debitApplicationRequest.NumberOfApprentices
                    ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new DebitApplicationCommandResult { IsSuccess = true });

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
                    command.Locations == _request.Locations &&
                    command.AdditionalLocation == _request.AdditionalLocation &&
                    command.SpecificLocation == _request.SpecificLocation &&
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
        public async Task Post_ApproveApplication_Approves_Application()
        {
            var request = _fixture.Create<ApproveApplicationRequest>();

            var actionResult = await _applicationsController.ApproveApplication(_pledgeId, _applicationId, request);
            var okResult = actionResult as OkResult;
            Assert.IsNotNull(okResult);

            _mediator.Verify(x => x.Send(It.Is<ApproveApplicationCommand>(command =>
                        command.PledgeId == _pledgeId &&
                        command.ApplicationId == _applicationId &&
                        command.UserId == request.UserId &&
                        command.UserDisplayName == request.UserDisplayName),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task Post_UndoApplicationApproval_Undoes_Application_Approval()
        {
            var actionResult = await _applicationsController.UndoApplicationApproval(_pledgeId, _applicationId);
            var okResult = actionResult as OkResult;
            Assert.IsNotNull(okResult);

            _mediator.Verify(x => x.Send(It.Is<UndoApplicationApprovalCommand>(command =>
                        command.PledgeId == _pledgeId &&
                        command.ApplicationId == _applicationId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
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
            var pledgeId = (int?)null;
            var applicationId = _fixture.Create<int>();
            var applicationResult = _fixture.Create<GetApplicationResult>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(applicationResult);

            // Act
            var actionResult = await _applicationsController.GetApplication(applicationId);
            var okObjectResult = actionResult as OkObjectResult;
            var getApplicationResponse = okObjectResult.Value as GetApplicationResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(getApplicationResponse);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Get_With_PledgeId_Returns_Application()
        {
            // Arrange
            var pledgeId = _fixture.Create<int>();
            var applicationId = _fixture.Create<int>();
            var applicationResult = _fixture.Create<GetApplicationResult>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)), It.IsAny<CancellationToken>()))
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
        public async Task Get_With_PledgeId_Returns_NotFound()
        {
            // Arrange
            var pledgeId = _fixture.Create<int>();
            var applicationId = _fixture.Create<int>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationResult)null);

            // Act
            var actionResult = await _applicationsController.GetApplication(pledgeId, applicationId);
            var okObjectResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Get_Returns_NotFound()
        {
            // Arrange
            var pledgeId = (int?)null;
            var applicationId = _fixture.Create<int>();

            _mediator
                .Setup(x => x.Send(It.Is<GetApplicationQuery>(y => (y.PledgeId == pledgeId) && (y.ApplicationId == applicationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetApplicationResult)null);

            // Act
            var actionResult = await _applicationsController.GetApplication(applicationId);
            var okObjectResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Get_Returns_Applications()
        {
            _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(query => query.PledgeId == _pledgeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetApplicationsResult(new LevyTransferMatching.Models.Application[]
            {
                new LevyTransferMatching.Models.Application()
            }));

            var actionResult = await _applicationsController.GetApplications(_pledgeId, null);
            var result = actionResult as OkObjectResult;
            Assert.IsNotNull(result);
            var response = result.Value as GetApplicationsResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Applications.Count());
        }

        [Test]
        public async Task Get_Returns_Applications_By_Account_Id()
        {
            _mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(query => query.AccountId == _accountId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetApplicationsResult(new LevyTransferMatching.Models.Application[]
            {
                new LevyTransferMatching.Models.Application()
            }));

            var actionResult = await _applicationsController.GetApplications(null, _accountId);
            var result = actionResult as OkObjectResult;
            Assert.IsNotNull(result);
            var response = result.Value as GetApplicationsResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Applications.Count());
        }

        [Test]
        public async Task Post_AcceptFunding_Returns_No_Content()
        {
            // Arrange
            var applicationId = _fixture.Create<int>();
            var accountId = _fixture.Create<long>();
            var request = _fixture.Create<AcceptFundingRequest>();
            var result = new AcceptFundingCommandResult()
            {
                Updated = true,
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<AcceptFundingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _applicationsController.AcceptFunding(applicationId, accountId, request);
            var noContentResult = actionResult as NoContentResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(noContentResult);
        }

        [Test]
        public async Task Post_AcceptFunding_Returns_BadRequest()
        {
            // Arrange
            var applicationId = _fixture.Create<int>();
            var accountId = _fixture.Create<long>();
            var request = _fixture.Create<AcceptFundingRequest>();
            var result = new AcceptFundingCommandResult()
            {
                Updated = false,
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<AcceptFundingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _applicationsController.AcceptFunding(applicationId, accountId, request);
            var badRequestResult = actionResult as BadRequestResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(badRequestResult);
        }

        [Test]
        public async Task Post_DebitApplication_Debits_Application()
        {
            var actionResult = await _applicationsController.DebitApplication(_applicationId, _debitApplicationRequest);
            var okResult = actionResult as OkResult;
            Assert.IsNotNull(okResult);

            _mediator.Verify(x => x.Send(It.Is<DebitApplicationCommand>(command =>
                        command.ApplicationId == _applicationId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
