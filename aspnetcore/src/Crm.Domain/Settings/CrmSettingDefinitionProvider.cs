using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Crm.Settings;

public class CrmSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(
                CrmSettings.UCardTotalSaleVolume,
                "216982",
                F("累计开卡数量"),
                F("用于前端显示的展示数据"))
            .WithProperty("Type", "number"));
    }

    private static FixedLocalizableString F(string name) => new(name);
}