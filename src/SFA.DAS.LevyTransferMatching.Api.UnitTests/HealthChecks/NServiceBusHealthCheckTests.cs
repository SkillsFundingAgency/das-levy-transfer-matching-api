using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NServiceBus;
using SFA.DAS.LevyTransferMatching.Api.HealthChecks;
using SFA.DAS.LevyTransferMatching.Messages.Commands;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.HealthChecks;

[TestFixture]
public class NServiceBusHealthCheckTests
{
    [Test]
    public async Task CheckHealthAsync_SendsHealthCheckToFunctionsEndpoint()
    {
        var messageSession = new Mock<IMessageSession>();
        var distributedCache = new Mock<IDistributedCache>();
        var destination = string.Empty;

        messageSession
            .Setup(session => session.Send(It.IsAny<RunHealthCheckCommand>(), It.IsAny<SendOptions>()))
            .Callback<object, SendOptions>((_, options) => destination = options.GetDestination())
            .Returns(Task.CompletedTask);

        distributedCache
            .Setup(cache => cache.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(System.Text.Encoding.UTF8.GetBytes("OK"));

        var healthCheck = new NServiceBusHealthCheck(messageSession.Object, distributedCache.Object);
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        result.Status.Should().Be(HealthStatus.Healthy);
        destination.Should().Be("SFA.DAS.LevyTransferMatching.Functions");
    }
}
