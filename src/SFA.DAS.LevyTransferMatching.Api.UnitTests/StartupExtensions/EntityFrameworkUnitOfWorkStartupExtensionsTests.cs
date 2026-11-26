using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Persistence;
using SFA.DAS.LevyTransferMatching.Api.StartupExtensions;
using SFA.DAS.LevyTransferMatching.Data;
using SFA.DAS.LevyTransferMatching.Infrastructure.Configuration;
using SFA.DAS.LevyTransferMatching.Infrastructure.ConnectionFactory;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.StartupExtensions;

[TestFixture]
public class EntityFrameworkUnitOfWorkStartupExtensionsTests
{
    [Test]
    public void AddEntityFrameworkForLevyTransferMatching_WithoutUnitOfWork_UsesConnectionFactory()
    {
        var connectionFactory = new Mock<IConnectionFactory>();
        var connection = new SqlConnection("Server=server;Initial Catalog=database;");
        connectionFactory
            .Setup(factory => factory.CreateConnection(It.IsAny<string>()))
            .Returns(connection);

        var unitOfWorkContext = new Mock<IUnitOfWorkContext>();
        unitOfWorkContext
            .Setup(context => context.Get<SynchronizedStorageSession>())
            .Throws<KeyNotFoundException>();

        var services = new ServiceCollection();
        services.AddSingleton(connectionFactory.Object);
        services.AddSingleton(unitOfWorkContext.Object);
        services.AddEntityFrameworkForLevyTransferMatching(new LevyTransferMatchingApi
        {
            DatabaseConnectionString = "Server=server;Initial Catalog=database;"
        });

        using var provider = services.BuildServiceProvider();
        var dbContext = provider.GetRequiredService<LevyTransferMatchingDbContext>();

        dbContext.Should().NotBeNull();
        connectionFactory.Verify(factory => factory.CreateConnection(It.IsAny<string>()), Times.Once);
    }
}
