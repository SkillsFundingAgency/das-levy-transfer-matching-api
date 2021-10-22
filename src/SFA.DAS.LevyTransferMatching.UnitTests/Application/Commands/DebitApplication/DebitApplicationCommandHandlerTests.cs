using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitApplication
{
    [TestFixture]
    public class DebitApplicationCommandHandlerTests
    {
        private DebitApplicationCommandHandler _handler;
        private Mock<IApplicationRepository> _repository;

        private readonly Fixture _fixture = new Fixture();
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
            _repository.Setup(x => x.Get(null, _application.Id, null)).ReturnsAsync(_application);

            _handler = new DebitApplicationCommandHandler(_repository.Object, Mock.Of<ILogger<DebitApplicationCommandHandler>>());
        }

        [TestCase(0, 100, 100)]
        [TestCase(100, 200, 300)]
        public async Task Handle_Application_AmountUsed_Is_Debited_By_Requested_Amount(int startValue, int debitValue, int expected)
        {
            _application.SetValue(x => x.AmountUsed, startValue);
            _command.Amount = debitValue;

            await _handler.Handle(_command, CancellationToken.None);

            _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.AmountUsed == expected)));
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 2, 3)]
        public async Task Handle_Application_NumberOfApprenticesUsed_Is_Debited_By_Requested_Amount(int startValue, int debitValue, int expected)
        {
            _application.SetValue(x => x.NumberOfApprenticesUsed, startValue);
            _command.NumberOfApprentices = debitValue;

            await _handler.Handle(_command, CancellationToken.None);

            _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.NumberOfApprenticesUsed == expected)));
        }

        [TestCase(3, 3, 4)]
        [TestCase(3, 5, 4)]
        [TestCase(3, 2, 0)]
        public async Task Handle_Application_Status_Is_Updated_When_Apprentice_Limit_Reached(int startValue, int debitValue, int expected)
        {
            _application.SetValue(x => x.NumberOfApprentices, startValue);
            _command.NumberOfApprentices = debitValue;

            await _handler.Handle(_command, CancellationToken.None);

            _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.Status == (ApplicationStatus)expected)));
        }

        [TestCase(300, 300, 4)]
        [TestCase(300, 500, 4)]
        [TestCase(300, 200, 0)]
        public async Task Handle_Application_Status_Is_Updated_When_Amount_Limit_Reached(int startValue, int debitValue, int expected)
        {
            _application.SetValue(x => x.Amount, startValue);
            _command.Amount = debitValue;

            await _handler.Handle(_command, CancellationToken.None);

            _repository.Verify(x => x.Update(It.Is<LevyTransferMatching.Data.Models.Application>(p => p == _application && p.Status == (ApplicationStatus)expected)));
        }
    }
}
