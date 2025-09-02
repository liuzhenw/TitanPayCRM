using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Astra;

public static class WebApplicationBuildHelper
{
    /// <summary>
    /// 运行 Web 程序
    /// </summary>
    /// <param name="args"></param>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <returns></returns>
    public static async Task<int> RunAsync<TStartupModule>(string[] args) where TStartupModule : IAbpModule
    {
        var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Host";
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.Console())
            .CreateLogger();
        Log.Information("{AppName} starting..", appName);

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsCommonJson()
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog(SerilogConfigurator.Configure);

            await builder.AddApplicationAsync<TStartupModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (HostAbortedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{AppName} terminated unexpectedly!", appName);
            return 1;
        }
        finally
        {
            Log.Information("{AppName} stopped!", appName);
            await Log.CloseAndFlushAsync();
        }
    }

    /// <summary>
    /// 构建请求处理管线
    /// </summary>
    /// <param name="context"></param>
    public static IApplicationBuilder BuildPipeline(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();
        app.UseUnitOfWork();
        app.UseAuthentication();
        app.UseAbpRequestLocalization();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            var configuration = context.GetConfiguration();
            var scopes = configuration.GetSection("Swagger:Auth:Scopes").Get<Dictionary<string, string>>() ?? [];
            options.SwaggerEndpoint("/swagger/v1/swagger.json", configuration["Swagger:Title"]);
            options.OAuthClientId(configuration["Swagger:Auth:ClientId"]);
            options.OAuthClientSecret(configuration["Swagger:Auth:ClientSecret"]);
            options.OAuthScopes(scopes.Select(s => s.Key).ToArray());
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
        return app;
    }

    /// <summary>
    /// 添加公共 JSON 配置
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <param name="optional"></param>
    /// <param name="reloadOnChange"></param>
    /// <returns></returns>
    public static IHostBuilder AddAppSettingsCommonJson(
        this IHostBuilder hostBuilder,
        bool optional = true,
        bool reloadOnChange = true)
    {
        return hostBuilder.ConfigureAppConfiguration((_, builder) => builder
            .AddJsonFile("appsettings.common.json", optional, reloadOnChange));
    }
}