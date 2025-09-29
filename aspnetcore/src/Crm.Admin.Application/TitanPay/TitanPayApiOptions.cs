namespace Crm.Admin.TitanPay;

public class TitanPayApiOptions
{
    public const string SectionName = "ExternalService:TitanPay";
    public string BaseUrl { get; set; } = "https://titanpay.vip/v1/open/";
    public string ShopId { get; set; } = "1";
}