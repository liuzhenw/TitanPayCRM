using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Crm.Admin.Products;

public class ProductBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
    public string? ImageUri { get; set; }
}

public class ProductDto : ProductBasicDto
{
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public uint SalesVolume { get; set; }
    public decimal SalesRevenue { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class CreateProductInput
{
    [Required]
    [StringLength(64)]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(64)]
    public string Name { get; set; } = null!;

    [StringLength(128)]
    public string? ImageUri { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}

public class UpdateProductInput
{
    [Required]
    [StringLength(64)]
    public string Name { get; set; } = null!;

    [StringLength(128)]
    public string? ImageUri { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}