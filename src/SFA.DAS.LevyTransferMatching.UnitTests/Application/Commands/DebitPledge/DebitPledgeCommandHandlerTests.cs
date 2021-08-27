using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitPledge
{
    [TestFixture]
    public class DebitPledgeCommandHandlerTests
    {
        private DebitPledgeCommandHandler _handler;
        private Mock<IPledgeRepository> _repository;

        private readonly Fixture _fixture = new Fixture();
        private DebitPledgeCommand _command;
        private Pledge _pledge;

        [SetUp]
        public void Setup()
        {
            _pledge = _fixture.Create<Pledge>();

            _command = new DebitPledgeCommand
            {
                PledgeId = _pledge.Id
            };

            _repository = new Mock<IPledgeRepository>();
            _repository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

            _handler = new DebitPledgeCommandHandler(_repository.Object, Mock.Of<ILogger<DebitPledgeCommandHandler>>());
        }

        [TestCase(1000, 100, 900)]
        [TestCase(1000, 1000, 0)]
        public async Task Handle_Pledge_Is_Debited_By_Requested_Amount(int startValue, int debitValue, int expectedRemaining)
        {
            _pledge.SetValue(x => x.RemainingAmount, startValue);
            _command.Amount = debitValue;

            await _handler.Handle(_command, CancellationToken.None);

            _repository.Verify(x => x.Update(It.Is<Pledge>(p => p == _pledge && p.RemainingAmount == expectedRemaining)));
        }

        [Test]
        public async Task Handle_Throws_If_Pledge_Cannot_Be_Debited_By_Amount()
        {
            _pledge.SetValue(x => x.RemainingAmount, 1000);
            _command.Amount = 1001;

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _handler.Handle(_command, CancellationToken.None));
        }
    }
}
