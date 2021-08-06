using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateAccount
{
    [TestFixture]
    
    public class CreateAccountCommandHandlerTests
    {
        private Fixture _fixture;
        private Mock<IEmployerAccountRepository> _employerAccountRepository;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _employerAccountRepository = new Mock<IEmployerAccountRepository>();

            _employerAccountRepository.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(() => null);
            _employerAccountRepository.Setup(x => x.Add(It.IsAny<EmployerAccount>()));
        }

        [Test]
        public async Task Handle_Account_Is_Created()
        {
            var handler = new CreateAccountCommandHandler(_employerAccountRepository.Object);
            var command = _fixture.Create<CreateAccountCommand>();

            await handler.Handle(command, CancellationToken.None);

            _employerAccountRepository.Verify(x => x.Add(It.Is<EmployerAccount>(a => a.Id == command.AccountId && a.Name == command.AccountName)), Times.Once);
        }
    }
}
