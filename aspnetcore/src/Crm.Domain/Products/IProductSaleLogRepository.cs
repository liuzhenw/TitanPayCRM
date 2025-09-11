using System;
using System.Threading.Tasks;
using Astra;

namespace Crm.Products;

public interface IProductSaleLogRepository :
    IAstraBasicWithPagedBasicRepository<ProductSaleLog, Guid, ProductSaleLogPagedParameter>
{
    Task<bool> ExistsAsync(string productId, string orderNo);
    Task<ProductSaleLog?> FindLastAsync(string productId);
}