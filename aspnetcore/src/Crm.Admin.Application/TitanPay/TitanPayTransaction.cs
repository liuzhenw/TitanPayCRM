namespace Crm.Admin.TitanPay;

public record TitanPayTransaction
{
    public required string Status { get; init; }
    public required string CardId { get; init; }
    public required string TransactionId { get; init; }
    public required string TransactionType { get; init; }
    public required string MaskedCardNumber { get; init; }
    public required decimal TransactionFee { get; init; }
    public required string TransactionFeeCurrency { get; init; }
    public required decimal BillingAmount { get; init; }
    public required string BillingCurrency { get; init; }
    public required long TransactionTime { get; init; }
    public required string? Description { get; init; }
    public required string? FailureReason { get; init; }
    public required TitanPayTransactionMerchant Merchant { get; init; }

    public bool IsValid =>
        Status == TitanPayTransactionStatus.Approved &&
        TransactionType == TitanPayTransactionType.Authorization &&
        BillingCurrency == "USD";
}

public record TitanPayTransactionMerchant
{
    public required string Name { get; init; }
    public required string Country { get; init; }
    public required string City { get; init; }
    public required string CategoryCode { get; init; }
}
public static class TitanPayTransactionStatus
{
    /// <summary>
    /// 处理中
    /// </summary>
    public const string Pending = "PENDING";
    /// <summary>
    /// 交易成功
    /// </summary>
    public const string Approved = "APPROVED";
    /// <summary>
    /// 交易失败
    /// </summary>
    public const string Failed = "FAILED";
    /// <summary>
    /// 被取消
    /// </summary>
    public const string Reversed = "REVERSED";
    /// <summary>
    /// 退款
    /// </summary>
    public const string Refunded = "REFUNDED";
    /// <summary>
    /// 清算
    /// </summary>
    public const string Cleared = "CLEARED";

}
public static class TitanPayTransactionType
{
    /// <summary>
    /// 交易
    /// </summary>
    public const string Authorization = "AUTHORIZATION";

    /// <summary>
    /// 清算
    /// </summary>
    public const string Clearing = "CLEARING";

    /// <summary>
    /// 退款
    /// </summary>
    public const string Refund = "REFUND";

    /// <summary>
    /// 撤销
    /// </summary>
    public const string Reversal = "REVERSAL";

    /// <summary>
    /// 充值
    /// </summary>
    public const string Recharge = "RECHARGE";

    /// <summary>
    /// 提款
    /// </summary>
    public const string Withdrawal = "WITHDRAWAL";

    /// <summary>
    /// 服务费
    /// </summary>
    public const string ServiceFee = "SERVICE_FEE";

    /// <summary>
    /// 验证
    /// </summary>
    public const string Verification = "VERIFICATION";
}