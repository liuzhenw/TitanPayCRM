using Volo.Abp.Reflection;

namespace Crm.Admin.Permissions;

public class CrmPermissions
{
    public const string GroupName = "Crm";

    public static class Settings
    {
        public const string Default = GroupName + ".Settings";
        public const string Update = Default + ".Update";
    }

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
    
    public static class ReferralRelations
    {
        public const string Default = GroupName + ".ReferralRelations";
    }

    public static class Referrers
    {
        public const string Default = GroupName + ".Referrers";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string ChangeCommission = Default + ".ChangeCommission";
    }

    public static class ReferralLevels
    {
        public const string Default = GroupName + ".ReferralLevels";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class ReferrerRequests
    {
        public const string Default = GroupName + ".ReferrerRequests";
        public const string Update = Default + ".Update";
    }

    public static class CommissionLogs
    {
        public const string Default = GroupName + ".CommissionLogs";
    }

    public static class WithdrawalRequests
    {
        public const string Default = GroupName + ".WithdrawalRequests";
        public const string Approve = Default + ".Approve";
        public const string Reject = Default + ".Reject";
    }


    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(CrmPermissions));
    }
}