using System;
using Astra.Paged;

namespace Crm.Referrals;

public class RecommendeeView
{
    public Guid Id { get; set; } 
    public string Email { get; set; } = null!;
    public string? LevelId { get; set; }
    public Guid RecommenderId { get; set; }
    public uint Depth { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class RecommendeeViewPagedParameter : BasicPagedParameter
{
    public Guid? RecommenderId { get; set; }
}