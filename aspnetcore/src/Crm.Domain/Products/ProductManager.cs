using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Crm.Accounts;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Crm.Products;

public class ProductManager(IProductSaleLogRepository logRepo) : DomainService
{
    public async Task SoldAsync(Product product, User customer, string orderNo, uint quantity, JsonObject data)
    {
        if (await logRepo.ExistsAsync(product.Id, orderNo))
            throw new UserFriendlyException($"{product.Name} 商品订单({orderNo})已存在!");

        var log = new ProductSaleLog(GuidGenerator.Create(), product, customer, orderNo, quantity, data);
        await logRepo.InsertAsync(log);
    }
}