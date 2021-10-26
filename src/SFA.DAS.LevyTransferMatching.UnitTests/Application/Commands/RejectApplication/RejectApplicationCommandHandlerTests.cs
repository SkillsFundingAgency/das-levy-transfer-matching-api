using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectApplication
{
    public class RejectApplicationCommandHandlerTests
    {
        private RejectApplicationCommandHandler _handler;
        private Mock<IApplicationRepository> _applicationRepository;

        private readonly Fixture _fixture = new Fixture();
        private LevyTransferMatching.Data.Models.Application _application;
        private RejectApplicationCommand _command;

        [SetUp]
        public void Setup()
        {
            _command = new RejectApplicationCommand
            {
                ApplicationId = _fixture.Create<int>(),
                PledgeId = _fixture.Create<int>(),
                UserId = _fixture.Create<string>(),
                UserDisplayName = _fixture.Create<string>()
            };

            _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(_command.PledgeId, _command.ApplicationId, null)).ReturnsAsync(_application);

            _applicationRepository.Setup(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(app => app == _application)))
                .Returns(Task.CompletedTask);

            _handler = new RejectApplicationCommandHandler(_applicationRepository.Object);
        }

        [Test]
        public async Task Handle_Application_Is_Marked_As_Rejected()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _applicationRepository.Verify(x =>
                x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(a => a == _application &&
                    a.Status == ApplicationStatus.Rejected &&
                    a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
        }
    }
}
