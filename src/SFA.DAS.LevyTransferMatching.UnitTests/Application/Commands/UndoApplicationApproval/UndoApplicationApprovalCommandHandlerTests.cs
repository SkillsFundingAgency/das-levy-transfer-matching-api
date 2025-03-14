﻿using SFA.DAS.LevyTransferMatching.Application.Commands.UndoApplicationApproval;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.UndoApplicationApproval;

[TestFixture]
public class UndoApplicationApprovalCommandHandlerTests
{
    private UndoApplicationApprovalCommandHandler _handler;
    private Mock<IApplicationRepository> _applicationRepository;

    private readonly Fixture _fixture = new();
    private LevyTransferMatching.Data.Models.Application _application;
    private UndoApplicationApprovalCommand _command;

    [SetUp]
    public void Setup()
    {
        _command = new UndoApplicationApprovalCommand
        {
            ApplicationId = _fixture.Create<int>(),
            PledgeId = _fixture.Create<int>()
        };

        _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
        _application.SetValue(x => x.Status, ApplicationStatus.Approved);

        _applicationRepository = new Mock<IApplicationRepository>();
        _applicationRepository.Setup(x => x.Get(_command.ApplicationId, _command.PledgeId, null)).ReturnsAsync(_application);

        _applicationRepository.Setup(x => x.Update(It.IsAny<LevyTransferMatching.Data.Models.Application>()))
            .Returns(Task.CompletedTask);

        _handler = new UndoApplicationApprovalCommandHandler(_applicationRepository.Object);
    }

    [Test]
    public async Task Handle_Application_Is_Marked_As_Pending()
    {
        await _handler.Handle(_command, CancellationToken.None);

        _applicationRepository.Verify(x =>
            x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(a => a == _application &&
                                                                              a.Status == ApplicationStatus.Pending &&
                                                                              a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
    }
}