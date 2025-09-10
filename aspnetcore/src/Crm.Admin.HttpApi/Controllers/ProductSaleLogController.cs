using Crm.Admin.Products;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Controllers;

[ApiController]
[Route("products/sale-logs")]
public class ProductSaleLogController(IProductSaleLogService saleLogService) : CrmAdminController
{
    [HttpGet]
    public Task<PagedResultDto<ProductSaleLogDto>> 
        GetListAsync([FromQuery] ProductSaleLogQueryInput input) => saleLogService.GetPagedListAsync(input);
    
    [HttpGet("{id:guid}")]
    public Task<ProductSaleLogDto> GetAsync(Guid id) => saleLogService.GetAsync(id);
}