using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreditPledge;

[TestFixture]
public class CreditPledgeCommandHandlerTests
{
    private CreditPledgeCommandHandler _handler;
    private Mock<IPledgeRepository> _repository;

    private readonly Fixture _fixture = new();
    private CreditPledgeCommand _command;
    private Pledge _pledge;

    [SetUp]
    public void Setup()
    {
        _pledge = _fixture.Create<Pledge>();

        _command = new CreditPledgeCommand
        {
            PledgeId = _pledge.Id
        };

        _repository = new Mock<IPledgeRepository>();
        _repository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

        _handler = new CreditPledgeCommandHandler(_repository.Object);
    }

    [TestCase(1000, 100, 1100)]
    [TestCase(1000, 1000, 2000)]
    public async Task Handle_Pledge_Is_Credited_By_Requested_Amount(int startValue, int creditValue, int expectedRemaining)
    {
        _pledge.SetValue(x => x.RemainingAmount, startValue);
        _command.Amount = creditValue;

        await _handler.Handle(_command, CancellationToken.None);

        _repository.Verify(x => x.Update(It.Is<Pledge>(p => p == _pledge && p.RemainingAmount == expectedRemaining)));
    }
}