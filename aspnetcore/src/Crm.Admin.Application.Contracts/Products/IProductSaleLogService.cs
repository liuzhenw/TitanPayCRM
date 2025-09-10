using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Crm.Admin.Products;

public interface IProductSaleLogService : IApplicationService
{
    Task<ProductSaleLogDto> GetAsync(Guid id);
    Task<PagedResultDto<ProductSaleLogDto>> GetPagedListAsync(ProductSaleLogQueryInput input);
}