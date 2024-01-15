using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Abstractions.CustomExceptions;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ClosePledge;

[TestFixture]
public class ClosePledgeCommandHandlerTests
{
    private ClosePledgeCommandHandler _handler;
    private Mock<IPledgeRepository> _repository;

    private readonly Fixture _fixture = new Fixture();
    private ClosePledgeCommand _command;
    private Pledge _pledge;

    [SetUp]
    public void Setup()
    {
        _pledge = _fixture.Create<Pledge>();

        _pledge.SetValue(x => x.Id, 1);
        _pledge.SetValue(x => x.Status, PledgeStatus.Active);

        _command = new ClosePledgeCommand
        {
            PledgeId = _pledge.Id,
            UserId = _fixture.Create<string>(),
            UserDisplayName = _fixture.Create<string>()
        };

        _repository = new Mock<IPledgeRepository>();
        _repository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

        _handler = new ClosePledgeCommandHandler(_repository.Object);
    }

    [Test]
    public async Task Handle_Pledge_Is_Closed()
    {
        _command.PledgeId = 1;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<Pledge>(p => p == _pledge && p.Status == PledgeStatus.Closed)));
    }

    [Test]
    public async Task Handle_Pledge_ClosedOn_Date_Is_Captured()
    {
        _command.PledgeId = 1;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<Pledge>(p => p == _pledge && p.ClosedOn.Value.Date.Equals(DateTime.UtcNow.Date))));
    }

    [Test]
    public void Handle_Pledge_Close_Error_If_Pledge_Id_Cannot_Be_Found()
    {
        _command.PledgeId = 2;

        var exception = Assert.ThrowsAsync<AggregateNotFoundException<Pledge>>(
            () => _handler.Handle(_command, CancellationToken.None));

        Assert.IsNotEmpty(exception.Message);
    }

    [Test]
    public void Handle_Pledge_Where_Pledge_Is_Already_Closed_Error()
    {
        _pledge.SetValue(x => x.Status, PledgeStatus.Closed);
        _repository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

        _command.PledgeId = 1;

        var exception = Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(_command, CancellationToken.None));

        Assert.IsNotEmpty(exception.Message);
    }
}