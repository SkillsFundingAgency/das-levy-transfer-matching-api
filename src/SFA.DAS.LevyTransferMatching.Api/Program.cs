using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;

namespace SFA.DAS.LevyTransferMatching.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseNServiceBusContainer()
            .UseNLog()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureKestrel(c => c.AddServerHeader = false)
                .UseStartup<Startup>();
            });
}