namespace Crm;

public static class CrmErrorCodes
{
    public const string Namespace = "Crm";

    public static class Accounts
    {
        public const string Group = Namespace + ":Accounts";
        public const string NotFound = Group + ":NotFound";
        public const string Locked = Group + ":Locked";
        public const string PasswordInvalid = Group + ":PasswordInvalid";
        public const string InvalidEvmAddress = Group + ":InvalidEvmAddress";
        public const string InvalidSignature = Group + ":InvalidSignature";
        public const string EmailAlreadyExist = Group + ":EmailAlreadyExist";
        public const string EmailAlreadyBound = Group + ":EmailAlreadyBound";
    }

    public static class Referrals
    {
        public const string Group = Namespace + ":Referrals";
        public const string CommissionInsufficient = Group + ":CommissionInsufficient";
        public const string WithdrawalAddressNotSet = Group + ":WithdrawalAddressNotSet";
        /// <summary>
        /// 推荐人身份请求已通过
        /// </summary>
        public const string ReferrerRequestIsApproved = Group + ":ReferrerRequestIsApproved";
        /// <summary>
        /// 暂时无法申请为此等级
        /// </summary>
        public const string CannotApplyThisLevel = Group + ":CannotApplyThisLevel";
        /// <summary>
        /// 禁止申请成为推荐人
        /// </summary>
        public const string RequestIsDisabled = Group + ":RequestIsDisabled";
        /// <summary>
        /// 申请未被拒绝
        /// </summary>
        public const string RequestNotRejected = Group + ":RequestNotRejected";
    }
}