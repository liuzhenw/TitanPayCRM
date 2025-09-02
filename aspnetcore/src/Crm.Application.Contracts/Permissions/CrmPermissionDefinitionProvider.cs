using Crm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Crm.Permissions;

public class CrmPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CrmPermissions.GroupName, L("Permission:Crm"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CrmResource>(name);
    }
}
