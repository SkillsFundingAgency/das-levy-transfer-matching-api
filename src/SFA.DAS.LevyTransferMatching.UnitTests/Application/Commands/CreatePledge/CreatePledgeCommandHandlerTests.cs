using System.Linq;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using SFA.DAS.LevyTransferMatching.Data.Models;
using Moq;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    [TestFixture]
    public class CreatePledgeCommandHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;
        private Mock<IEmployerAccountRepository> _employerAccountRepository;
        private Mock<IPledgeRepository> _pledgeRepository;

        private CreatePledgeCommandHandler _handler;

        private EmployerAccount _employerAccount;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _employerAccountRepository = new Mock<IEmployerAccountRepository>();
            _pledgeRepository = new Mock<IPledgeRepository>();

            _employerAccount = _fixture.Create<EmployerAccount>();

            _employerAccountRepository.Setup(x => x.Get(_employerAccount.Id)).ReturnsAsync(_employerAccount);

            _handler = new CreatePledgeCommandHandler(_employerAccountRepository.Object, _pledgeRepository.Object);
            
        }

        [Test]
        public async Task Handle_Pledge_Is_Created()
        {
            var command = _fixture.Create<CreatePledgeCommand>();
            command.AccountId = _employerAccount.Id;

            Pledge inserted = null;

            _pledgeRepository.Setup(x => x.Add(It.IsAny<Pledge>()))
                .Callback<Pledge>(r => inserted = r);

            await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(inserted);
            Assert.AreEqual(command.AccountId, inserted.EmployerAccount.Id);
            Assert.AreEqual(command.Amount, inserted.Amount);
            Assert.AreEqual(command.Amount, inserted.RemainingAmount);
            Assert.AreEqual(command.IsNamePublic, inserted.IsNamePublic);
            Assert.AreEqual(command.AutomaticApprovalOption, inserted.AutomaticApprovalOption);
            Assert.AreEqual((Level)command.Levels.Cast<int>().Sum(), inserted.Levels);
            Assert.AreEqual((Sector)command.Sectors.Cast<int>().Sum(), inserted.Sectors);
            Assert.AreEqual((JobRole)command.JobRoles.Cast<int>().Sum(), inserted.JobRoles);

            var compareLogic = new CompareLogic(new ComparisonConfig{ IgnoreCollectionOrder = true, IgnoreObjectTypes = true, IgnoreUnknownObjectTypes = true });
            var expectedLocations = command.Locations.Select(l => new PledgeLocation { Name =  l.Name, Latitude = l.Geopoint[0], Longitude = l.Geopoint[1] }).ToList();
            var result = compareLogic.Compare(expectedLocations, inserted.Locations);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }
    }
}