using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApproveApplication;

[TestFixture]
public class ApproveApplicationCommandHandlerTests
{
    private ApproveApplicationCommandHandler _handler;
    private Mock<IApplicationRepository> _applicationRepository;

    private readonly Fixture _fixture = new();
    private Data.Models.Application _application;
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

        _application = _fixture.Build<Data.Models.Application>()
            .Create();

        _applicationRepository = new Mock<IApplicationRepository>();
        _applicationRepository.Setup(x => x.Get(_command.ApplicationId, _command.PledgeId, null)).ReturnsAsync(_application);

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

    [Test]
    public async Task Handle_On_Approval_When_Application_Under_New_Cost_Model_Amount_Is_Correct()
    {
        _application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);

        await _handler.Handle(_command, CancellationToken.None);

        var events = _application.FlushEvents();
        Assert.That(events.Any(x => x is ApplicationApproved approvalEvent && approvalEvent.Amount == _application.GetCost()), Is.True);
    }
}