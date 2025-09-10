using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Crm.Admin.Permissions;

public class CrmPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(CrmPermissions.GroupName, F("CRM"));

        var settings = group.AddPermission(CrmPermissions.Settings.Default, F("系统设置"));
        settings.AddChild(CrmPermissions.Settings.Update, F("修改"));
        
        var users = group.AddPermission(CrmPermissions.Users.Default, F("用户"));
        users.AddChild(CrmPermissions.Users.Create, F("创建"));
        users.AddChild(CrmPermissions.Users.Update, F("修改"));
        users.AddChild(CrmPermissions.Users.Delete, F("删除"));

        var roles = group.AddPermission(CrmPermissions.Roles.Default, F("角色"));
        roles.AddChild(CrmPermissions.Roles.Create, F("创建"));
        roles.AddChild(CrmPermissions.Roles.Update, F("修改"));
        roles.AddChild(CrmPermissions.Roles.Delete, F("删除"));

        var products = group.AddPermission(CrmPermissions.Products.Default, F("商品"));
        products.AddChild(CrmPermissions.Products.Create, F("创建"));
        products.AddChild(CrmPermissions.Products.Update, F("修改"));
        products.AddChild(CrmPermissions.Products.Delete, F("删除"));
        products.AddChild(CrmPermissions.Products.SaleLogs, F("销售记录"));

        var referrers = group.AddPermission(CrmPermissions.Referrers.Default, F("推销员"));
        referrers.AddChild(CrmPermissions.Referrers.Create, F("创建"));
        referrers.AddChild(CrmPermissions.Referrers.Update, F("修改"));

        var levels = group.AddPermission(CrmPermissions.ReferralLevels.Default, F("推荐等级"));
        levels.AddChild(CrmPermissions.ReferralLevels.Create, F("创建"));
        levels.AddChild(CrmPermissions.ReferralLevels.Update, F("修改"));
        levels.AddChild(CrmPermissions.ReferralLevels.Delete, F("删除"));

        var requests = group.AddPermission(CrmPermissions.ReferrerRequests.Default, F("推广申请"));
        requests.AddChild(CrmPermissions.ReferrerRequests.Update, F("修改"));

        var commissionLogs = group.AddPermission(CrmPermissions.CommissionLogs.Default, F("佣金记录"));

    }

    private static FixedLocalizableString F(string name)
    {
        return new FixedLocalizableString(name);
    }
}