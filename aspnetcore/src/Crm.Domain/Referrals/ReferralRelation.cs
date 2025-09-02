using System;
using Crm.Accounts;
using Volo.Abp.Domain.Entities;

namespace Crm.Referrals;

public class ReferralRelation : BasicAggregateRoot<Guid>
{
    protected ReferralRelation() { }

    public ReferralRelation(User recommender, User recommendee, uint depth)
    {
        RecommenderId = recommender.Id;
        RecommenderEmail = recommender.Email;
        RecommendeeId = recommendee.Id;
        RecommendeeEmail = recommendee.Email;
        Depth = depth;
        CreatedAt = DateTimeOffset.Now;
    }

    public Guid RecommenderId { get; private set; }
    public Guid RecommendeeId { get; private set; }
    public string RecommenderEmail { get; private set; } = null!;
    public string RecommendeeEmail { get; private set; } = null!;
    public uint Depth { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}