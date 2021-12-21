using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ClosePledge
{

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
                PledgeId = _pledge.Id
            };

            _repository = new Mock<IPledgeRepository>();
            _repository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

            _handler = new ClosePledgeCommandHandler(_repository.Object, Mock.Of<ILogger<ClosePledgeCommandHandler>>());
        }

        [TestCase(1)]
        public async Task Handle_Pledge_Is_Closed(int pledgeId)
        {
            _command.PledgeId = pledgeId;

            await _handler.Handle(_command, CancellationToken.None);
            
            _repository.Verify(x => x.Update(It.Is<Pledge>(p => p == _pledge && p.Status == PledgeStatus.Closed)));
        }

        [TestCase(2)]
        [TestCase(null)]
        public async Task Handle_Pledge_Close_Error_If_Pledge_Id_Cannot_Be_Found_Or_Null(int pledgeId)
        {
            if(pledgeId == 0) {
                _command = null;
            } else {
                _command.PledgeId = pledgeId;
            }
            
            var result = await _handler.Handle(_command, CancellationToken.None);
            
            Assert.IsFalse(result.Updated);
        }
    }
}
