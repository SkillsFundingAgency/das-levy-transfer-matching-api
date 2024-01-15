using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.AcceptFunding;

public class AcceptFundingCommandHandlerTests
{
    private AcceptFundingCommandHandler _handler;
    private Mock<IApplicationRepository> _repository;
    private AcceptFundingCommand _command;
    private readonly Fixture _fixture = new();
    private Data.Models.Application _application;
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
        _application = _fixture.Create<Data.Models.Application>();
        _application.SetValue(o => o.Status, ApplicationStatus.Approved);
        _repository = new Mock<IApplicationRepository>();
        _repository.Setup(x => x.Get(_command.ApplicationId, null, _command.AccountId))
            .ReturnsAsync(_application);

        _handler = new AcceptFundingCommandHandler(_repository.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_Application_Sets_Status_Correctly()
    {
        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x =>
            x.Update(It.Is<Data.Models.Application>(a => a == _application &&
                                                         a.Status == ApplicationStatus.Accepted &&
                                                         a.UpdatedOn.Value.Date == DateTime.UtcNow.Date)));
    }
}