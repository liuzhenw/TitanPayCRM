using Astra;
using Crm;
using Scalar.AspNetCore;
using Serilog;

// https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder();
builder.Host
    .AddAppSettingsCommonJson()
    .AddAppSettingsSecretsJson()
    .UseAutofac()
    .UseSerilog(SerilogConfigurator.Configure);
builder.Services.AddOpenApi();
await builder.AddApplicationAsync<CrmWebApiModule>();
var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();
await app.InitializeApplicationAsync();
return await WebApplicationHelper.RunAsync(app);