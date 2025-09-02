using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace Astra;

public static class ServiceConfigurationContextExtensions
{
    public static ServiceConfigurationContext ConfigureByConvention(this ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        return context
            .ConfigureAuthentication(configuration)
            .ConfigureSwagger(configuration)
            .ConfigureCors(configuration)
            .ConfigureCookie()
            .ConfigureForwardedHeaders()
            .ConfigureLocalization()
            .ConfigureAuditing(configuration)
            .ConfigureDistributedCache(configuration)
            .ConfigureDataProtection(configuration);
    }

    public static ServiceConfigurationContext ConfigureAuditing(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.Configure<AbpAuditingOptions>(options =>
        {
            options.ApplicationName = configuration["App:ApplicationName"]!;
            options.IsEnabledForAnonymousUsers = false;
            options.IsEnabledForGetRequests = false;
            options.IsEnabledForIntegrationServices = false;
        });
        return context;
    }
    
    public static ServiceConfigurationContext ConfigStringEncryption(
        this ServiceConfigurationContext context,IConfiguration configuration)
    {
        var passPhrase = configuration["StringEncryption:DefaultPassPhrase"] ?? "dS3o1Q3$vI6CtF0#";
        var initVector = configuration["StringEncryption:InitVector"] ?? "mSK347lrG0idJvt4";
        var salt = configuration["StringEncryption:DefaultSalt"] ?? "lN7xqB6*";
        context.Services.Configure<AbpStringEncryptionOptions>(opts =>
        {
            opts.DefaultPassPhrase = passPhrase;
            opts.DefaultSalt = Encoding.ASCII.GetBytes(salt);
            opts.InitVectorBytes = Encoding.ASCII.GetBytes(initVector);
            opts.Keysize = 256;
        });
        return context;
    }

    public static ServiceConfigurationContext ConfigureSwagger(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        var scopes = configuration.GetSection("Swagger:Auth:Scopes").Get<Dictionary<string, string>>() ?? [];
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            scopes,
            options =>
            {
                var version = configuration["Swagger:Version"];
                var apiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = configuration["Swagger:Title"],
                    Description = configuration["Swagger:Description"]
                };

                options.SwaggerDoc(version, apiInfo);
                options.DocInclusionPredicate((_, _) => true);
                options.CustomSchemaIds(type => type.FullName);
                if (bool.TryParse(configuration["Swagger:HideAbp"], out var hideAbp) && hideAbp)
                    options.HideAbpEndpoints();
            });
        return context;
    }

    public static ServiceConfigurationContext ConfigureLocalization(
        this ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });
        return context;
    }

    public static ServiceConfigurationContext ConfigureAuthentication(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = configuration["AuthServer:Audience"];
            });
        return context;
    }

    public static ServiceConfigurationContext ConfigureDataProtection(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        var appNamespace = configuration["App:ApplicationName"] ?? Assembly.GetEntryAssembly()?.GetName().Name!;
        context.Services
            .AddDataProtection()
            .SetApplicationName(appNamespace)
            .PersistKeysToFileSystem(new DirectoryInfo("data-protection-keys"));
        return context;
    }
    

    public static ServiceConfigurationContext ConfigureDistributedCache(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        var appNamespace = configuration["App:ApplicationName"]!;
        context.Services.Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = appNamespace + ":";
            options.HideErrors = false;
            options.GlobalCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                SlidingExpiration = TimeSpan.FromHours(2),
            };
        });
        return context;
    }

    public static ServiceConfigurationContext ConfigureCors(
        this ServiceConfigurationContext context, IConfiguration configuration)
    {
        var origins = configuration.GetSection("App:Cors:Origins").Get<string[]>()               ?? [];
        var exposedHeaders = configuration.GetSection("App:Cors:ExposedHeaders").Get<string[]>() ?? [];
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                if (origins.Length > 0)
                    builder
                        .WithOrigins(
                            origins
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowCredentials();
                else
                    builder.AllowAnyOrigin();
                if (exposedHeaders.Length > 0)
                    builder.WithHeaders(exposedHeaders);
                builder
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        return context;
    }

    public static ServiceConfigurationContext ConfigureForwardedHeaders(this ServiceConfigurationContext context)
    {
        context.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            options.KnownNetworks.Add(IPNetwork.Parse("192.168.0.0/16"));
            options.KnownNetworks.Add(IPNetwork.Parse("172.16.0.0/12"));
        });
        return context;
    }
    
    public static ServiceConfigurationContext ConfigureCookie(this ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
            options.TokenCookie.SameSite = SameSiteMode.Lax;
            options.TokenCookie.HttpOnly = true;
        });
        context.Services.Configure<CookieOptions>(options => { options.SameSite = SameSiteMode.Lax; });
        return context;
    }
}