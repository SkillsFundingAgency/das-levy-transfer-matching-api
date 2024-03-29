﻿using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitApplication;

[TestFixture]
public class DebitApplicationCommandHandlerTests
{
    private DebitApplicationCommandHandler _handler;
    private Mock<IApplicationRepository> _repository;

    private readonly Fixture _fixture = new();
    private DebitApplicationCommand _command;
    private LevyTransferMatching.Data.Models.Application _application;

    [SetUp]
    public void Setup()
    {
        _application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();

        _command = new DebitApplicationCommand
        {
            ApplicationId = _application.Id
        };

        _repository = new Mock<IApplicationRepository>();
        _repository.Setup(x => x.Get(_application.Id, null, null)).ReturnsAsync(_application);

        _handler = new DebitApplicationCommandHandler(_repository.Object);
    }

    [TestCase(0, 100, 100)]
    [TestCase(100, 200, 300)]
    public async Task Handle_Application_AmountUsed_Is_Debited_By_Requested_Amount(int startValue, int debitValue, int expected)
    {
        _application.SetValue(x => x.Status, ApplicationStatus.Accepted);
        _application.SetValue(x => x.AmountUsed, startValue);
        _command.Amount = debitValue;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.AmountUsed == expected)));
    }

    [TestCase(0, 1, 1)]
    [TestCase(1, 2, 3)]
    public async Task Handle_Application_NumberOfApprenticesUsed_Is_Debited_By_Requested_Amount(int startValue, int debitValue, int expected)
    {
        _application.SetValue(x => x.Status, ApplicationStatus.Accepted);
        _application.SetValue(x => x.NumberOfApprenticesUsed, startValue);
        _command.NumberOfApprentices = debitValue;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.NumberOfApprenticesUsed == expected)));
    }

    [TestCase(3, 3, 4)]
    [TestCase(3, 5, 4)]
    [TestCase(3, 2, 3)]
    public async Task Handle_Application_Status_Is_Updated_When_Apprentice_Limit_Reached(int startValue, int debitValue, int expected)
    {
        _application.SetValue(x => x.Status, ApplicationStatus.Accepted);
        _application.SetValue(x => x.NumberOfApprentices, startValue);

        _command.NumberOfApprentices = debitValue;
        _command.MaxAmount = 1000;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.Status == (ApplicationStatus)expected)));
    }

    [TestCase(0, 300, 300, 4)]
    [TestCase(0, 500, 300, 4)]
    [TestCase(300, 100, 400, 4)]
    [TestCase(0, 200, 300, 3)]
    [TestCase(0, 200, 500, 3)]
    [TestCase(300, 200, 1000, 3)]
    public async Task Handle_Application_Status_Is_Updated_When_Amount_Limit_Reached(int initialAmountUsed, int debitAmount, int maxAmount, int expected)
    {
        _application.SetValue(x => x.Status, ApplicationStatus.Accepted);
        _application.SetValue(x => x.AmountUsed, initialAmountUsed);

        _command.MaxAmount = maxAmount;
        _command.Amount = debitAmount;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.Status == (ApplicationStatus)expected)));
    }
}