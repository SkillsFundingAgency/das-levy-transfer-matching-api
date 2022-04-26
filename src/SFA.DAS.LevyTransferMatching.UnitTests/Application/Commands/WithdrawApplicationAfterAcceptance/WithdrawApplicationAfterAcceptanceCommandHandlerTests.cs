using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommandHandlerTests
    {
        private WithdrawApplicationAfterAcceptanceCommandHandler _handler;
        private Mock<IApplicationRepository> _applicationRepository;
        private Mock<IPledgeRepository> _pledgeRepository;

        private readonly Fixture _fixture = new Fixture();
        private WithdrawApplicationAfterAcceptanceCommand _command;
        private LevyTransferMatching.Data.Models.Application _application;
        private LevyTransferMatching.Data.Models.Pledge _pledge;

        [SetUp]
        public void SetUp()
        {
            _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            _application.SetValue(x => x.Status, ApplicationStatus.Accepted);

            _pledge = _fixture.Create<LevyTransferMatching.Data.Models.Pledge>();

            _command = new WithdrawApplicationAfterAcceptanceCommand
            {
                ApplicationId = _application.Id,
                UserId = _fixture.Create<string>(),
                UserDisplayName = _fixture.Create<string>()
            };

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(_application.Id, null, null)).ReturnsAsync(_application);
            _applicationRepository.Setup(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(x => x.Id == _application.Id)))
                .Returns(Task.CompletedTask);

            _pledgeRepository = new Mock<IPledgeRepository>();
            _pledgeRepository.Setup(x => x.Get(_application.PledgeId)).ReturnsAsync(_pledge);

            _handler = new WithdrawApplicationAfterAcceptanceCommandHandler(_applicationRepository.Object, _pledgeRepository.Object);
        }

        [Test]
        public async Task Withdraw_Application_After_Acceptance_Is_Marked_As_Approved()
        {
            var expectedPledgeRemainingAmount = _pledge.RemainingAmount + _application.Amount;

            await _handler.Handle(_command, CancellationToken.None);

            Assert.AreEqual(ApplicationStatus.WithdrawnAfterAcceptance, _application.Status);
            Assert.AreEqual(expectedPledgeRemainingAmount, _pledge.RemainingAmount);
        }
    }
}
