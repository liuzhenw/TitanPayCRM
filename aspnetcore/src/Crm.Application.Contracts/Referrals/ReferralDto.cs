using System;
using System.Collections.Generic;
using Crm.Products;
using Crm.Users;
using FluentValidation;
using Volo.Abp.Application.Dtos;

namespace Crm.Referrals;

public class ReferralLevelBasicDto : EntityDto<string>
{
    public string Name { get; set; } = null!;
}

public class ReferralLevelDto : ReferralLevelBasicDto
{
    public decimal Multiplier { get; set; }
    public string? Description { get; set; }
    public uint Size { get; set; }
}

public class ReferrerDto : EntityDto<Guid>
{
    public ReferralLevelBasicDto? Level { get; set; }
    public uint DirectCount { get; set; }
    public uint IndirectCount { get; set; }
    public uint TotalCount { get; set; }
    public decimal TotalCommission { get; set; }
    public decimal Commission { get; set; }
    public decimal Withdrawal { get; set; }
    public string? WithdrawalAddress { get; set; }
    public List<ReferrerSaleStatisticDto> Statistics { get; set; } = null!;
}

public class ReferrerSaleStatisticDto
{
    public ProductBasicDto Product { get; set; } = null!;
    /// <summary>
    /// 销售量
    /// </summary>
    public uint Volume { get; set; }
    /// <summary>
    /// 销售额
    /// </summary>
    public decimal Revenue { get; set; }
    /// <summary>
    /// 佣金
    /// </summary>
    public decimal Commission { get; set; }
}

public class ReferrerWithdrawalAddressUpdateInput
{
    public string Address { get; set; } = null!;
}

public class ReferrerWithdrawalAddressUpdateInputValidator : AbstractValidator<ReferrerWithdrawalAddressUpdateInput>
{
    public ReferrerWithdrawalAddressUpdateInputValidator()
    {
        RuleFor(x => x.Address).Matches(@"^0x[a-fA-F0-9]{40}$");
    }
}

public class ReferrerRequestDto : EntityDto<Guid>
{
    public ReferralLevelBasicDto Level { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? RejectReason { get; set; }
}

public class ReferrerApplyInput
{
    public string LevelId { get; set; } = null!;
}

public class RecommendeeDto : EntityDto<Guid>
{
    public string Email { get; set; } = null!;
    public uint ConsumptionCount { get; set; }
    public decimal TotalConsumption { get; set; }
    public ReferralLevelBasicDto? Level { get; set; }
    public string RecommenderEmail { get; set; } = null!;
    public uint Depth { get; set; }
    public decimal TotalCommission { get; set; }
    public long CreatedAt { get; set; }
    public List<ReferrerSaleStatisticDto>? Statistics { get; set; }
}

public class RecommendeeQueryInput : PagedAndSortedResultRequestDto
{
    public string? LevelId { get; set; }
    public uint? Depth { get; set; }
}

public class AncestorQueryModelDto
{
    public uint Depth { get; set; }
    public UserBasicDto User { get; set; } = null!;
    public ReferralLevelBasicDto? Level { get; set; }
}