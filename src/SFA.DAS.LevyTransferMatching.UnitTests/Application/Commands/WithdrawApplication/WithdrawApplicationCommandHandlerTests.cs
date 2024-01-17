using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.WithdrawApplication;

[TestFixture]
public class WithdrawApplicationCommandHandlerTests
{
    private WithdrawApplicationCommandHandler _handler;
    private Mock<IApplicationRepository> _applicationRepository;

        private readonly Fixture _fixture = new Fixture();
        private WithdrawApplicationCommand _command;
        private Data.Models.Application _application;
        private UserInfo _userInfo;
        private Mock<IPledgeRepository> _pledgeRepository;
        private Data.Models.Pledge _pledge;

        [SetUp]
        public void SetUp()
        {
            _application = _fixture.Create<Data.Models.Application>();
            _userInfo = _fixture.Create<UserInfo>();
            _application = _fixture.Create<Data.Models.Application>();
            _pledge = _fixture.Create<Data.Models.Pledge>();

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
            
            _pledgeRepository = new Mock<IPledgeRepository>();
            _pledgeRepository.Setup(x => x.Get(_application.PledgeId))
                .ReturnsAsync(_pledge);


            _handler = new WithdrawApplicationCommandHandler(_applicationRepository.Object);
        }

    [Test]
    public async Task Withdraw_Application_Is_Marked_As_Withdrawn()
    {
        await _handler.Handle(_command, CancellationToken.None);

        _applicationRepository.Verify(x =>
            x.Update(It.Is<Data.Models.Application>(a => a == _application &&
                                                         a.Status == ApplicationStatus.Withdrawn &&
                                                         a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
    }

        [Test]
        public async Task Withdraw_Application_When_Accepted_Then_Application_Is_Marked_As_WithdrawnAfterAcceptance()
        {
            _application.Approve(_userInfo);
            _application.AcceptFunding(_userInfo, false);

        await _handler.Handle(_command, CancellationToken.None);

        _applicationRepository.Verify(x =>
            x.Update(It.Is<Data.Models.Application>(a => a == _application &&
                                                         a.Status == ApplicationStatus.WithdrawnAfterAcceptance &&
                                                         a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
    }

        [Test]
        public async Task Handle_When_Application_Withdrawn_After_Acceptance_Under_New_Cost_Model_Amount_Is_Correct()
        {
            _application.Approve(_userInfo);
            _application.AcceptFunding(_userInfo, false);

        _application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);

        await _handler.Handle(_command, CancellationToken.None);

        var events = _application.FlushEvents();
        Assert.That(events.Any(x => x is ApplicationWithdrawnAfterAcceptance approvalEvent && approvalEvent.Amount == _application.GetCost()), Is.True);
    }
}