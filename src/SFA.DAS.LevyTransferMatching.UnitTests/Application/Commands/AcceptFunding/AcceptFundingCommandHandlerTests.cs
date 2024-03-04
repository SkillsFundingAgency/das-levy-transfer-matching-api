using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Domain.Events;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.AcceptFunding;

public class AcceptFundingCommandHandlerTests
{
    private AcceptFundingCommandHandler _handler;
    private Mock<IApplicationRepository> _applicationRepository;
    private Mock<IPledgeRepository> _pledgeRepository;
    private AcceptFundingCommand _command;
    private readonly Fixture _fixture = new();
    private Data.Models.Application _application;
    private Data.Models.Pledge _pledge;
    private Mock<ILogger<AcceptFundingCommandHandler>> _logger;

    [SetUp]
    public void Setup()
    {
        _command = new AcceptFundingCommand
        {
            ApplicationId = 1,
            AccountId = 1,
            UserId = "userId",
            UserDisplayName = "userName"
        };

        _logger = new Mock<ILogger<AcceptFundingCommandHandler>>();

        _pledge = _fixture.Create<Data.Models.Pledge>();
        _application = _fixture.Create<Data.Models.Application>();
        _application.SetValue(o => o.Status, ApplicationStatus.Approved);
        _applicationRepository = new Mock<IApplicationRepository>();
        _applicationRepository.Setup(x => x.Get(_command.ApplicationId, null, _command.AccountId))
            .ReturnsAsync(_application);
        _pledgeRepository = new Mock<IPledgeRepository>();
        _pledgeRepository.Setup(x => x.Get(_application.PledgeId))
            .ReturnsAsync(_pledge);

        _handler = new AcceptFundingCommandHandler(_applicationRepository.Object, _pledgeRepository.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_Application_Sets_Status_Correctly()
    {
        await _handler.Handle(_command, CancellationToken.None);

        _applicationRepository.Verify(x =>
            x.Update(It.Is<Data.Models.Application>(application => application == _application 
                                                                   && application.Status == ApplicationStatus.Accepted 
                                                                   && application.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
    }

    [Test]
    public async Task Handle_Application_Publishes_ApplicationFundingAcceptedEvent()
    {
        await _handler.Handle(_command, CancellationToken.None);

        var events = _application.FlushEvents();
        Assert.That(events.Any(x => x is ApplicationFundingAccepted), Is.True);
    }
}