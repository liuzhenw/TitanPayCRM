using Crm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Crm.Admin.Permissions;

public class CrmPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(CrmPermissions.GroupName, F("CRM"));

        var users = group.AddPermission(CrmPermissions.Users.Default, F("用户"));
        users.AddChild(CrmPermissions.Users.Create, F("创建"));
        users.AddChild(CrmPermissions.Users.Update, F("修改"));
        users.AddChild(CrmPermissions.Users.Delete, F("删除"));
        
        var roles = group.AddPermission(CrmPermissions.Users.Default, F(" 角色"));
        roles.AddChild(CrmPermissions.Roles.Create, F("创建"));
        roles.AddChild(CrmPermissions.Roles.Update, F("修改"));
        roles.AddChild(CrmPermissions.Roles.Delete, F("删除"));
        
    }
    
    private static FixedLocalizableString F(string name)
    {
        return new FixedLocalizableString(name);
    }
}
