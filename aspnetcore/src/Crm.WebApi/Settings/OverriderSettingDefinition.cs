using Volo.Abp.Emailing;
using Volo.Abp.Settings;

namespace Crm.Settings;

public class OverriderSettingDefinition : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.GetOrNull(EmailSettingNames.Smtp.Port)?.WithProperty("Type", "number");
        context.GetOrNull(EmailSettingNames.Smtp.EnableSsl)?.WithProperty("Type", "boolean");
        context.GetOrNull(EmailSettingNames.Smtp.UseDefaultCredentials)?.WithProperty("Type", "boolean");
    }
}