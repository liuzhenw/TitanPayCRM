using Volo.Abp.Application.Dtos;

namespace Crm.Products;

public class ProductBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
}