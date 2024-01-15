using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.Data.Enums;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Services;
using SFA.DAS.LevyTransferMatching.Testing;
using SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplication;

public class GetApplicationQueryHandlerTests : LevyTransferMatchingDbContextFixture
{
    private Fixture _fixture;
    private Mock<IDateTimeService> _dateTimeService;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _dateTimeService = new Mock<IDateTimeService>();
        _dateTimeService.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
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

        var handler = new GetApplicationQueryHandler(DbContext, _dateTimeService.Object);

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

        var handler = new GetApplicationQueryHandler(DbContext, _dateTimeService.Object);

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
        var handler = new GetApplicationQueryHandler(DbContext, _dateTimeService.Object);

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
        var handler = new GetApplicationQueryHandler(DbContext, _dateTimeService.Object);

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


    [Test]
    public async Task Handle_When_CostingModel_Is_OneYear_Then_Amount_Is_Correct()
    {
        // Arrange
        var pledge = _fixture.Create<Data.Models.Pledge>();
        var employerAccount = _fixture.Create<Data.Models.EmployerAccount>();
        var createPledgeApplicationProperties = _fixture.Create<CreateApplicationProperties>();
        var userInfo = _fixture.Create<UserInfo>();

        _fixture.Register(
            () => new Data.Models.Application(pledge, employerAccount, createPledgeApplicationProperties, userInfo));

        var application = _fixture.Create<Data.Models.Application>();
        application.SetValue(x => x.CostingModel, ApplicationCostingModel.OneYear);
        await DbContext.AddAsync(application);
        await DbContext.SaveChangesAsync();

        var handler = new GetApplicationQueryHandler(DbContext, _dateTimeService.Object);

        var applicationId = application.Id;
        var request = new GetApplicationQuery()
        {
            ApplicationId = applicationId,
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Amount, Is.EqualTo(application.GetCost()));
    }

}