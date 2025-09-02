using Volo.Abp.Reflection;

namespace Crm.Permissions;

public class CrmPermissions
{
    public const string GroupName = "Crm";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CrmPermissions));
    }
}
