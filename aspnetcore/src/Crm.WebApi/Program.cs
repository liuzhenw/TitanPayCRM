using Astra;
using Crm;

// https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

return await WebApplicationHelper.RunAsync<CrmWebApiModule>(args);