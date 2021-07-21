using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateApplication
{
    [TestFixture]
    public class CreateApplicationCommandHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;
        private Mock<IEmployerAccountRepository> _employerAccountRepository;
        private Mock<IPledgeRepository> _pledgeRepository;
        private Mock<IApplicationRepository> _applicationRepository;

        private CreateApplicationCommandHandler _handler;

        private EmployerAccount _employerAccount;
        private Pledge _pledge;

        private LevyTransferMatching.Data.Models.Application inserted;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _employerAccountRepository = new Mock<IEmployerAccountRepository>();
            _pledgeRepository = new Mock<IPledgeRepository>();
            _applicationRepository = new Mock<IApplicationRepository>();

            _employerAccount = _fixture.Create<EmployerAccount>();
            _pledge = _fixture.Create<Pledge>();

            _employerAccountRepository.Setup(x => x.Get(_employerAccount.Id)).ReturnsAsync(_employerAccount);
            _pledgeRepository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

            _applicationRepository.Setup(x => x.Add(It.IsAny<LevyTransferMatching.Data.Models.Application>()))
                .Callback<LevyTransferMatching.Data.Models.Application>(r => inserted = r);

            _handler = new CreateApplicationCommandHandler(_pledgeRepository.Object, _applicationRepository.Object, _employerAccountRepository.Object, DbContext);
        }

        [Test]
        public async Task Handle_Application_Is_Created()
        {
            var command = _fixture.Create<CreateApplicationCommand>();
            command.ReceiverEmployerAccountId = _employerAccount.Id;
            command.PledgeId = _pledge.Id;
            command.EmployerAccountId = _pledge.EmployerAccount.Id;

            await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(inserted);
            Assert.AreEqual(command.EmployerAccountId, inserted.Pledge.EmployerAccount.Id);
            Assert.AreEqual(command.PledgeId, inserted.Pledge.Id);
            Assert.AreEqual(command.ReceiverEmployerAccountId, inserted.EmployerAccount.Id);
        }
    }
}
