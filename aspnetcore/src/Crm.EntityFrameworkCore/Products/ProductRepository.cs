using System.Threading.Tasks;
using Astra.EntityFrameworkCore;
using Crm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Crm.Products;

public class ProductRepository(IDbContextProvider<CrmDbContext> dbContextProvider) :
    AstraEfCoreRepository<CrmDbContext, Product, string>(dbContextProvider),
    IProductRepository
{
    public async Task UpdateStatisticAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlAsync(
            $"""
             UPDATE "Products" p
             SET "TotalCommission" = t.total, "SalesVolume" = t.volume, "SalesRevenue" = t.revenue
             FROM (
                 SELECT "ProductId", SUM("TotalCommission") AS total, SUM("Quantity") AS volume, SUM("Amount") AS revenue
                 FROM "ProductSaleLogs"
                 GROUP BY "ProductId"
             ) t
             WHERE p."Id" = t."ProductId";                               
             """);
    }
}