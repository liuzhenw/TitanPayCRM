using Volo.Abp.Reflection;

namespace Crm.Admin.Permissions;

public class CrmPermissions
{
    public const string GroupName = "Crm";

    public static class Users
    {
        public const string Default = GroupName + ".Users";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    
    public static class Roles
    {
        public const string Default = GroupName + ".Roles";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    
    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string SaleLogs = Default + ".SaleLogs";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CrmPermissions));
    }
}