using System.Security.Claims;
using Volo.Abp.Authorization.Permissions;

namespace Astra.Permissions;

public class RootPermissionValueProvider(IPermissionStore permissionStore) : PermissionValueProvider(permissionStore)
{
    public override string Name => "Root";

    public override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var result = PermissionGrantResult.Undefined;
        if (IsRootRole(context.Principal))
            result = PermissionGrantResult.Granted;

        return Task.FromResult(result);
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var isRoot = IsRootRole(context.Principal);
        var result = new MultiplePermissionGrantResult();
        foreach (var item in context.Permissions)
        {
            result.Result[item.Name] = isRoot
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }

        return Task.FromResult(result);
    }

    private static bool IsRootRole(ClaimsPrincipal? principal)
    {
        return principal?.IsInRole(AstraConsts.RootRole) ?? false;
    }
}