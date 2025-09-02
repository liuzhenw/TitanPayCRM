using System;
using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Products;

public class ProductSaleLogRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraPagedEfCoreRepository<CrmDbContext, ProductSaleLog, Guid, ProductSaleLogPagedParameter>(dbContextProvider),
    IProductSaleLogRepository
{
    public async Task<bool> ExistsAsync(string productId, string orderNo)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<ProductSaleLog>()
            .AnyAsync(x => x.ProductId == productId && x.OrderNo == orderNo);
    }
}