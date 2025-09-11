using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Astra;

public static class SerilogConfigurator
{
    public static void Configure(HostBuilderContext context, LoggerConfiguration configuration)
    {
        //configuration.ReadFrom.Configuration(context.Configuration);
        const string logOutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}";
        configuration
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
            .MinimumLevel.Override("OpenIddict", LogEventLevel.Warning)
            .MinimumLevel.Override("Quartz", LogEventLevel.Warning)
            .MinimumLevel.Override("Castle", LogEventLevel.Warning)
            .MinimumLevel.Override("Polly", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
#if !DEBUG
                .WriteTo.Async(
                    c => c.File(
                        "Logs/logs.log",
                        LogEventLevel.Warning,
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 1024 * 1024 * 5,
                        rollOnFileSizeLimit: true))
#endif
            .WriteTo.Async(c => c.Console(outputTemplate:logOutputTemplate));
    }
}