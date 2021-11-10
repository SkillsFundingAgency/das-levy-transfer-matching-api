using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandlerTests : LevyTransferMatchingDbContextFixture
    {
        private Fixture _fixture;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _fixture = new Fixture();
        }

        [Test]
        public async Task Handle_Application_Found_Returns_Result()
        {
            // Arrange
            var pledge = _fixture.Create<LevyTransferMatching.Data.Models.Pledge>();
            var employerAccount = _fixture.Create<LevyTransferMatching.Data.Models.EmployerAccount>();
            var createPledgeApplicationProperties = _fixture.Create<CreateApplicationProperties>();
            var userInfo = _fixture.Create<UserInfo>();

            _fixture.Register(
                () => new LevyTransferMatching.Data.Models.Application(pledge, employerAccount, createPledgeApplicationProperties, userInfo));

            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.AddAsync(application);

            await DbContext.SaveChangesAsync();

            var handler = new GetApplicationQueryHandler(DbContext);

            var applicationId = application.Id;
            var request = new GetApplicationQuery()
            {
                ApplicationId = applicationId,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Handle_Application_With_PledgeId_Found_Returns_Result()
        {
            // Arrange
            var pledge = _fixture.Create<LevyTransferMatching.Data.Models.Pledge>();
            var employerAccount = _fixture.Create<LevyTransferMatching.Data.Models.EmployerAccount>();
            var createPledgeApplicationProperties = _fixture.Create<CreateApplicationProperties>();
            var userInfo = _fixture.Create<UserInfo>();

            _fixture.Register(
                () => new LevyTransferMatching.Data.Models.Application(pledge, employerAccount, createPledgeApplicationProperties, userInfo));

            var application = _fixture.Create<LevyTransferMatching.Data.Models.Application>();
            await DbContext.AddAsync(application);

            await DbContext.SaveChangesAsync();

            var handler = new GetApplicationQueryHandler(DbContext);

            var applicationId = application.Id;
            var pledgeId = pledge.Id;
            var request = new GetApplicationQuery()
            {
                ApplicationId = applicationId,
                PledgeId = pledgeId,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Handle_Application_Not_Found_Returns_Null()
        {
            // Arrange
            var handler = new GetApplicationQueryHandler(DbContext);

            var applicationId = _fixture.Create<int>();
            var request = new GetApplicationQuery()
            {
                ApplicationId = applicationId,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Handle_Application_With_PledgeId_Not_Found_Returns_Null()
        {
            // Arrange
            var handler = new GetApplicationQueryHandler(DbContext);

            var applicationId = _fixture.Create<int>();
            var pledgeId = _fixture.Create<int>();
            var request = new GetApplicationQuery()
            {
                ApplicationId = applicationId,
                PledgeId = pledgeId,
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }
    }
}