using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApproveApplication
{
    [TestFixture]
    public class ApproveApplicationCommandHandlerTests
    {
        private ApproveApplicationCommandHandler _handler;
        private Mock<IApplicationRepository> _applicationRepository;

        private readonly Fixture _fixture = new Fixture();
        private LevyTransferMatching.Data.Models.Application _application;
        private ApproveApplicationCommand _command;

        [SetUp]
        public void Setup()
        {
            _command = new ApproveApplicationCommand
            {
                ApplicationId = _fixture.Create<int>(),
                PledgeId = _fixture.Create<int>(),
                UserId = _fixture.Create<string>(),
                UserDisplayName = _fixture.Create<string>()
            };

            _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(_command.PledgeId, _command.ApplicationId)).ReturnsAsync(_application);

            _applicationRepository.Setup(x => x.Update(It.IsAny<LevyTransferMatching.Data.Models.Application>()))
                .Returns(Task.CompletedTask);

            _handler = new ApproveApplicationCommandHandler(_applicationRepository.Object);
        }

        [Test]
        public async Task Handle_Application_Is_Marked_As_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _applicationRepository.Verify(x =>
                x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(a => a == _application &&
                    a.Status == ApplicationStatus.Approved &&
                    a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
        }
    }
}
