using System.Linq;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Domain.Models
{
    [TestFixture]
    public class PledgeTests
    {
        private readonly Fixture _autoFixture = new Fixture();

        [Test]
        public void On_Create_Ledger_Entry_With_Opening_Balance_Is_Created()
        {
            var account = _autoFixture.Create<EmployerAccount>();
            var properties = _autoFixture.Create<CreatePledgeProperties>();
            var userInfo = _autoFixture.Create<UserInfo>();

            var pledge = new Pledge(account, properties, userInfo);

            var ledgerEntry = pledge.Ledger.First();

            Assert.AreEqual(properties.Amount, ledgerEntry.Amount);
            Assert.AreEqual(properties.Amount, ledgerEntry.Balance);
            Assert.AreEqual(UserAction.CreatePledge.ToString(), ledgerEntry.UserAction);
        }
    }
}
