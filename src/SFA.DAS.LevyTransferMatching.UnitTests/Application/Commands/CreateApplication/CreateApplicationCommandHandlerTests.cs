using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Data.Models;
using SFA.DAS.LevyTransferMatching.Data.Repositories;
using SFA.DAS.LevyTransferMatching.Models.Enums;
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

        private LevyTransferMatching.Data.Models.Application _inserted;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _fixture = new Fixture();

            _employerAccountRepository = new Mock<IEmployerAccountRepository>();
            _pledgeRepository = new Mock<IPledgeRepository>();
            _applicationRepository = new Mock<IApplicationRepository>();

            _employerAccount = _fixture.Create<EmployerAccount>();
            _pledge = _fixture.Create<Pledge>();

            _employerAccountRepository.Setup(x => x.Get(_employerAccount.Id)).ReturnsAsync(_employerAccount);
            _pledgeRepository.Setup(x => x.Get(_pledge.Id)).ReturnsAsync(_pledge);

            _applicationRepository.Setup(x => x.Add(It.IsAny<LevyTransferMatching.Data.Models.Application>()))
                .Callback<LevyTransferMatching.Data.Models.Application>(r => _inserted = r);

            _handler = new CreateApplicationCommandHandler(_pledgeRepository.Object, _applicationRepository.Object, _employerAccountRepository.Object);
        }

        [Test]
        public async Task Handle_Application_Is_Created()
        {
            var command = _fixture.Create<CreateApplicationCommand>();
            command.EmployerAccountId = _employerAccount.Id;
            command.PledgeId = _pledge.Id;
            command.Locations = _pledge.Locations.Select(x => x.Id).ToList();

            await _handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(_inserted);
            Assert.AreEqual(command.PledgeId, _inserted.Pledge.Id);
            Assert.AreEqual(command.EmployerAccountId, _inserted.EmployerAccount.Id);
            Assert.AreEqual(command.Details, _inserted.Details);
            Assert.AreEqual(command.StandardId, _inserted.StandardId);
            Assert.AreEqual(command.StandardTitle, _inserted.StandardTitle);
            Assert.AreEqual(command.StandardLevel, _inserted.StandardLevel);
            Assert.AreEqual(command.StandardDuration, _inserted.StandardDuration);
            Assert.AreEqual(command.StandardMaxFunding, _inserted.StandardMaxFunding);
            Assert.AreEqual(command.StandardRoute, _inserted.StandardRoute);
            Assert.AreEqual(command.NumberOfApprentices, _inserted.NumberOfApprentices);
            Assert.AreEqual(command.StartDate, _inserted.StartDate);
            Assert.AreEqual(command.HasTrainingProvider, _inserted.HasTrainingProvider);
            Assert.AreEqual(command.Amount, _inserted.Amount);
            Assert.AreEqual((Sector)command.Sectors.Cast<int>().Sum(), _inserted.Sectors);
            CollectionAssert.AreEqual(command.Locations, _inserted.ApplicationLocations.Select(x => x.PledgeLocationId).ToList());
            Assert.AreEqual(command.AdditionalLocation, _inserted.AdditionalLocation);
            Assert.AreEqual(command.SpecificLocation, _inserted.SpecificLocation);
            Assert.AreEqual(command.FirstName, _inserted.FirstName);
            Assert.AreEqual(command.LastName, _inserted.LastName);
            Assert.AreEqual(command.BusinessWebsite, _inserted.BusinessWebsite);
            Assert.AreEqual(command.NumberOfApprentices * command.StandardMaxFunding, _inserted.TotalAmount);
            CollectionAssert.AreEqual(command.EmailAddresses, _inserted.EmailAddresses.Select(x=> x.EmailAddress));
        }
    }
}
