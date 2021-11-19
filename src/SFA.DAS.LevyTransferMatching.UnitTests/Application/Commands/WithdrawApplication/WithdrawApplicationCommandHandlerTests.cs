using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.WithdrawApplication
{
    [TestFixture]
    public class WithdrawApplicationCommandHandlerTests
    {
        private WithdrawApplicationCommandHandler _handler;
        private Mock<IApplicationRepository> _applicationRepository;

        private readonly Fixture _fixture = new Fixture();
        private WithdrawApplicationCommand _command;
        private LevyTransferMatching.Data.Models.Application _application;

        [SetUp]
        public void SetUp()
        {
            _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();

            _command = new WithdrawApplicationCommand
            {
                ApplicationId = _application.Id,
                AccountId = _fixture.Create<long>(),
                UserId = _fixture.Create<string>(),
                UserDisplayName = _fixture.Create<string>()
            };

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(_application.Id, null, _command.AccountId)).ReturnsAsync(_application);
            _applicationRepository.Setup(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(x => x.Id == _application.Id)))
                .Returns(Task.CompletedTask);

            _handler = new WithdrawApplicationCommandHandler(_applicationRepository.Object);
        }

        [Test]
        public async Task Withdraw_Application_Is_Marked_As_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _applicationRepository.Verify(x =>
                x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(a => a == _application &&
                    a.Status == ApplicationStatus.Withdrawn &&
                    a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
        }
    }
}
