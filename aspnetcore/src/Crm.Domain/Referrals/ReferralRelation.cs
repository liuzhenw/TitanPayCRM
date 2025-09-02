using System;
using System.Collections.Generic;
using System.Linq;
using Astra.Paged;
using Crm.Accounts;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace Crm.Referrals;

public class ReferralRelation : BasicAggregateRoot<Guid>
{
    protected ReferralRelation() { }

    public ReferralRelation(User recommender, User recommendee, uint depth)
    {
        Recommender = new ReferralRelationUser(recommender.Id, recommender.Email);
        Recommendee = new ReferralRelationUser(recommendee.Id, recommendee.Email);
        Depth = depth;
        CreatedAt = DateTimeOffset.Now;
    }

    public ReferralRelationUser Recommender { get; private set; } = null!;
    public ReferralRelationUser Recommendee { get; private set; } = null!;
    public uint Depth { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}

public class ReferralRelationUser : ValueObject
{
    protected ReferralRelationUser() { }

    public ReferralRelationUser(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Email;
    }
}

public class ReferralRelationPagedParameter : PagedParameter<ReferralRelation>
{
    public Guid? RecommenderId { get; set; }
    public Guid? RecommendeeId { get; set; }
    public uint? MinDepth { get; set; }

    public override IQueryable<ReferralRelation> BuildPagedQueryable(IQueryable<ReferralRelation> queryable)
    {
        return queryable
            .WhereIf(RecommenderId.HasValue, x => x.Recommender.Id == RecommenderId)
            .WhereIf(RecommendeeId.HasValue, x => x.Recommendee.Id == RecommendeeId)
            .WhereIf(MinDepth.HasValue, x => x.Depth >= MinDepth);
    }
}