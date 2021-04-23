using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LevyTransferMatching.Functions.StartupExtensions
{
    public static class ServiceProviderExtensions
    {
        public static ILogger GetLogger(this ServiceProvider serviceProvider, string typeName) => serviceProvider.GetService<ILoggerProvider>().CreateLogger(typeName);
        public static IConfiguration GetConfiguration(this ServiceProvider serviceProvider) => serviceProvider.GetService<IConfiguration>();
    }
}
