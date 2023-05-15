using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.WithdrawApplication
{
    [TestFixture]
    public class WithdrawApplicationCommandHandlerTests
    {
        private WithdrawApplicationCommandHandler _handler;
        private Mock<IApplicationRepository> _applicationRepository;

        private readonly Fixture _fixture = new Fixture();
        private WithdrawApplicationCommand _command;
        private Data.Models.Application _application;

        [SetUp]
        public void SetUp()
        {
            _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            _application.SetValue(x => x.Amount, _fixture.Create<int>());

            var userInfo = _fixture.Create<UserInfo>();
            _application = _fixture.Create<Data.Models.Application>();
            _application.SetValue(x => x.Amount, _fixture.Create<int>());

            // Make it 'accepted', otherwise withdrawing will fail
            _application.Approve(userInfo);
            _application.AcceptFunding(userInfo);

            _command = new WithdrawApplicationCommand
            {
                ApplicationId = _application.Id,
                AccountId = _fixture.Create<long>(),
                UserId = _fixture.Create<string>(),
                UserDisplayName = _fixture.Create<string>()
            };

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.Get(_application.Id, null, _command.AccountId)).ReturnsAsync(_application);
            _applicationRepository.Setup(x => x.Update(It.Is<Data.Models.Application>(x => x.Id == _application.Id)))
                .Returns(Task.CompletedTask);

            _handler = new WithdrawApplicationCommandHandler(_applicationRepository.Object);
        }

        [Test]
        public async Task Withdraw_Application_Is_Marked_As_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _applicationRepository.Verify(x =>
                x.Update(It.Is<Data.Models.Application>(a => a == _application &&
                    a.Status == ApplicationStatus.Withdrawn &&
                    a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
        }

        [Test]
        public async Task Handle_When_Application_Withdrawn_After_Acceptance_Under_New_Cost_Model_Amount_Is_Correct()
        {
            _application.SetValue(x => x.Status, ApplicationStatus.Accepted);
            _application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);

            await _handler.Handle(_command, CancellationToken.None);

            var events = _application.FlushEvents();
            Assert.IsTrue(events.Any(x => x is ApplicationWithdrawnAfterAcceptance approvalEvent && approvalEvent.Amount == _application.Amount));
        }
    }
}
