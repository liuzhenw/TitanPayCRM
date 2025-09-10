using System;
using System.Collections.Generic;

namespace Crm.Referrals;

public class AncestorQueryModel
{
    public Guid Id { get; set; }
    public uint Depth { get; set; }
    public string? LevelId { get; set; }
    public decimal TotalCommission { get; set; }
    public List<SaleStatistic>? Statistics { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}