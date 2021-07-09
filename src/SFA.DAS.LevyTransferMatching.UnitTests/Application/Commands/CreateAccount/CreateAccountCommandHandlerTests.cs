using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateAccount;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateAccount
{
    [TestFixture]
    
    public class CreateAccountCommandHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Account_Is_Created()
        {
            var handler = new CreateAccountCommandHandler(DbContext);
            var command = _fixture.Create<CreateAccountCommand>();

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var insertedAccount = DbContext.EmployerAccounts.Find(command.AccountId);
            Assert.IsNotNull(insertedAccount);

            Assert.AreEqual(command.AccountId, insertedAccount.Id);
            Assert.AreEqual(command.AccountName, insertedAccount.Name);
        }
    }
}
