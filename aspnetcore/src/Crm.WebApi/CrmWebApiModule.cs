using System.Text;
using Astra;
using Astra.Permissions;
using Crm.Admin;
using Crm.EntityFrameworkCore;
using Crm.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Crm;

[DependsOn(typeof(AstraAspNetCoreModule),
    typeof(CrmAdminHttpApiModule),
    typeof(CrmAdminApplicationModule),
    typeof(CrmEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule))]
public class CrmWebApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var services = context.Services;

        context
            .ConfigureCors(configuration)
            .ConfigureCookie()
            .ConfigureForwardedHeaders()
            .ConfigureAuditing(configuration)
            .ConfigStringEncryption(configuration);

        ConfigureAuthentication(services, configuration);
        ConfigurePermission(services);
        ConfigureLocalization(services);
        ConfigureSwagger(services, configuration);
        ConfigureRoutePrefix(services);
        Configure<AbpDbContextOptions>(options => options.UseNpgsql());
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
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
            options.SwaggerEndpoint("/swagger/v1/swagger.json", configuration["Swagger:Title"]);
        });
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5),
                    ValidIssuer = configuration["Authorization:Issuer"],
                    ValidAudience = configuration["Authorization:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Authorization:Secret"]!))
                };
            });
    }
    
    private static void ConfigurePermission(IServiceCollection services)
    {
        services.Configure<AbpPermissionOptions>(options =>
        {
            options.ValueProviders.Add<RootPermissionValueProvider>();
        });
    }

    private static void ConfigureLocalization(IServiceCollection services)
    {
        services.Configure<AbpLocalizationOptions>(options =>
        {
            options.DefaultResourceType = typeof(CrmResource);
            options.Languages.Add(new LanguageInfo("en-US", "en-US", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁体中文"));
        });
    }

    private static void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAbpSwaggerGen(options =>
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
    }

    private static void ConfigureRoutePrefix(IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            options.Conventions.Insert(0, new RoutePrefixConvention<CrmController>());
            options.Conventions.Insert(1, new RoutePrefixConvention<CrmAdminController>("/api/admin"));
        });
    }
}