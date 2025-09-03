using Crm.Admin.Permissions;
using Crm.Products;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Products;

[Authorize(CrmPermissions.Products.SaleLogs)]
public class ProductSaleLogService(IProductSaleLogRepository repo) : CrmAdminAppService, IProductSaleLogService
{
    public async Task<PagedResultDto<ProductSaleLogDto>> GetPagedListAsync(ProductSaleLogQueryInput input)
    {
        var parameter = new ProductSaleLogPagedParameter();
        ObjectMapper.Map(input, parameter);
        var paged = await repo.GetPagedListAsync(parameter);
        return new PagedResultDto<ProductSaleLogDto>(
            paged.TotalCount,
            ObjectMapper.Map<List<ProductSaleLog>, List<ProductSaleLogDto>>(paged.Items));
    }
}