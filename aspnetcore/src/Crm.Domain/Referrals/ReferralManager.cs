using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Accounts;
using Crm.Products;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Crm.Referrals;

public class ReferralManager(
    IUserRepository userRepo,
    IReferrerRepository referrerRepo,
    IReferralRelationRepository relationRepo,
    IReferrerLevelRepository levelRepo,
    ICommissionLogRepository commissionRepo,
    IWithdrawalRequestRepository withdrawalRepo,
    IReferrerRequestRepository requestRepo
) : DomainService
{
    /// <summary>
    /// 申请成为推荐人
    /// </summary>
    /// <param name="user"></param>
    /// <param name="levelId"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async Task<ReferrerRequest> ReferrerApplyingAsync(User user, string levelId)
    {
        var request = await requestRepo.FindAsync(user.Id);
        if (request is { Status: ReferrerRequestStatus.Approved })
            throw new BusinessException(CrmErrorCodes.Referrals.ReferrerRequestIsApproved);

        var level = await levelRepo.GetAsync(levelId);
        await ValidateReferrerRequest(user, level);

        if (request is null)
        {
            request = new ReferrerRequest(user, level);
            await requestRepo.InsertAsync(request);
        }
        else
        {
            request.Reset(level);
            await requestRepo.UpdateAsync(request);
        }

        return request;
    }

    /// <summary>
    /// 同意成为推荐人的申请
    /// </summary>
    /// <param name="request"></param>
    /// <param name="auditorId"></param>
    public async Task<Referrer> ApproveReferrerRequestAsync(ReferrerRequest request, Guid auditorId)
    {
        request.Approve(auditorId);
        var requester = await userRepo.GetAsync(request.Id);
        var level = await levelRepo.GetAsync(request.LevelId);
        await ValidateReferrerRequest(requester, level);
        await requestRepo.UpdateAsync(request);

        var referrer = new Referrer(request);
        await referrerRepo.InsertAsync(referrer);

        level.OnUserJoined();
        await levelRepo.UpdateAsync(level);
        return referrer;
    }

    /// <summary>
    /// 创建推荐人
    /// </summary>
    /// <param name="user"></param>
    /// <param name="levelId"></param>
    public async Task<Referrer> CreateReferrerAsync(User user, string? levelId)
    {
        ReferralLevel? level = null;
        if (levelId is not null) level = await levelRepo.GetAsync(levelId);
        var referrer = new Referrer(user, level);
        await referrerRepo.InsertAsync(referrer);
        if (level is not null)
        {
            level.OnUserJoined();
            await levelRepo.UpdateAsync(level);
        }

        return referrer;
    }

    /// <summary>
    /// 修改推荐人的等级
    /// </summary>
    /// <param name="referrer"></param>
    /// <param name="newLevelId"></param>
    /// <exception cref="UserFriendlyException"></exception>
    /// <exception cref="BusinessException"></exception>
    public async Task ModifyReferrerLevelAsync(Referrer referrer, string newLevelId)
    {
        var newLevel = await levelRepo.GetAsync(newLevelId);
        ReferralLevel? curLevel = null;
        if (referrer.LevelId is not null) curLevel = await levelRepo.GetAsync(referrer.LevelId);
        if (newLevel == curLevel) return;
        // 下调:确保等级不能低于下级
        if (curLevel is not null && newLevel < curLevel)
        {
            var maxLevelDescendant = await referrerRepo.FindMaxLevelDescendantAsync(referrer.Id);
            if (maxLevelDescendant?.LevelId != null)
            {
                var minLevel = await levelRepo.GetAsync(maxLevelDescendant.LevelId);
                if (newLevel <= minLevel)
                    throw new UserFriendlyException($"不能低于 {minLevel.Name} 等级!");
            }
        }
        // 上调:确保等级不能高于上级
        else
        {
            var parent = await referrerRepo.FindParentAsync(referrer.Id);
            if (parent?.LevelId is null) throw new UserFriendlyException("等级不能高于上级!");

            var maxLevel = await levelRepo.GetAsync(parent.LevelId);
            if (newLevel >= maxLevel)
                throw new BusinessException($"不能高于 {maxLevel} 等级!");
        }

        if (curLevel is not null)
        {
            curLevel.OnUserQuited();
            await levelRepo.UpdateAsync(curLevel);
        }

        referrer.SetLevel(newLevel);
        newLevel.OnUserJoined();
        await levelRepo.UpdateAsync(newLevel);
    }

    /// <summary>
    /// 添加推荐关系
    /// </summary>
    /// <param name="recommender"></param>
    /// <param name="recommendee"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task AddRelationAsync(User recommender, User recommendee)
    {
        if (recommender.Id == recommendee.Id)
            throw new UserFriendlyException("推荐人不能是自己!");

        if (await relationRepo.ExistsAsync(recommendee.Id))
            throw new UserFriendlyException($"{recommendee.Email} 已有推荐人!");

        // 处理直推
        var referrer = await referrerRepo.FindAsync(recommender.Id);
        if (referrer is null)
        {
            referrer = new Referrer(recommender, null);
            await referrerRepo.InsertAsync(referrer, true);
        }

        referrer.OnDirectReferralAdded();
        var relation = new ReferralRelation(recommender, recommendee, 1);
        List<Referrer> referrers = [referrer];
        List<ReferralRelation> relations = [relation];
        // 处理间推
        var ancestorRelations = await relationRepo.GetAncestorListAsync(recommender.Id, 1);
        foreach (var item in ancestorRelations.OrderBy(x => x.Depth))
        {
            var ancestorUser = await userRepo.GetAsync(item.Recommender.Id);
            var ancestorRelation = new ReferralRelation(ancestorUser, recommendee, item.Depth + 1u);
            relations.Add(ancestorRelation);

            var ancestor = await referrerRepo.GetAsync(item.Recommender.Id);
            ancestor.OnIndirectReferralAdded();
            referrers.Add(ancestor);
        }

        await relationRepo.InsertManyAsync(relations);
        await referrerRepo.UpdateManyAsync(referrers);
    }

    public async Task<WithdrawalRequest> CreateWithdrawalRequestAsync(Referrer referrer, decimal amount)
    {
        if (referrer.WithdrawalAddress.IsNullOrWhiteSpace())
            throw new BusinessException(CrmErrorCodes.Referrals.WithdrawalAddressNotSet);

        if (referrer.Commission < amount)
            throw new BusinessException(CrmErrorCodes.Referrals.CommissionInsufficient);

        var request = new WithdrawalRequest(GuidGenerator.Create(), referrer, amount, referrer.WithdrawalAddress);
        await withdrawalRepo.InsertAsync(request);

        referrer.OnWithdrawal(request);
        await referrerRepo.UpdateAsync(referrer);
        return request;
    }

    public async Task RefundWithdrawalAsync(WithdrawalRequest request, string reason, Guid auditorId)
    {
        request.Reject(reason, auditorId);
        await withdrawalRepo.UpdateAsync(request);

        var referrer = await referrerRepo.GetAsync(request.ReferrerId);
        referrer.OnRefundWithdrawal(request);
        await referrerRepo.UpdateAsync(referrer);
    }

    public async Task<ReferralLevel> CreateReferralLevelAsync(string id, string name, uint size, decimal multiplier)
    {
        if (await levelRepo.ExistsAsync(size))
            throw new UserFriendlyException($"大小为 {size} 的等级已存在!");

        return new ReferralLevel(id, name, size, multiplier);
    }

    public async Task ModifyReferralLevelSizeAsync(ReferralLevel level, uint newSize)
    {
        if (level.Size == newSize) return;

        if (await levelRepo.ExistsAsync(newSize))
            throw new UserFriendlyException($"大小为 {newSize} 的等级已存在!");

        // 调小
        if (level.Size > newSize && level.UserCount > 0)
            throw new UserFriendlyException("此等级下已有用户,无法调整大小!");

        level.SetSize(newSize);
    }
    
    /// <summary>
    /// 商品售出时触发
    /// </summary>
    /// <param name="saleLog"></param>
    internal async Task OnProductSoldAsync(ProductSaleLog saleLog)
    {
        var ancestorRelations = await relationRepo.GetAncestorListAsync(saleLog.CustomerId, 1);
        if (ancestorRelations.Count < 1) return;

        var levels = await levelRepo.GetListAsync();
        List<ReferralLevel> updatedLevels = [];
        foreach (var relation in ancestorRelations)
        {
            var referrer = await referrerRepo.GetAsync(relation.Recommender.Id);
            if (referrer.IsDisabled || referrer.LevelId is null) continue;

            var level = levels.First(x => x.Id == referrer.LevelId);
            var commission = saleLog.Amount * level.Multiplier;
            var commissionLog = new CommissionLog(GuidGenerator.Create(), referrer, relation, saleLog, commission);
            await commissionRepo.InsertAsync(commissionLog);
            referrer.OnCommissionAdded(saleLog, commission);
            level.OnCommissionAdded(commission);
            level.UpdatedAt = DateTimeOffset.Now;
            await referrerRepo.UpdateAsync(referrer);

            if (updatedLevels.All(x => x.Id != level.Id))
                updatedLevels.Add(level);
        }

        if (updatedLevels.Count > 0)
            await levelRepo.UpdateManyAsync(levels);
    }

    private async Task ValidateReferrerRequest(User user, ReferralLevel level)
    {
        var parentRelations = await relationRepo.FindParentAsync(user.Id);
        if (parentRelations is not null)
        {
            var parent = await referrerRepo.FindAsync(parentRelations.Recommender.Id);
            // 上级不是推荐人身份,下级也不能成为推荐人
            if (parent?.LevelId is null)
                throw new BusinessException(CrmErrorCodes.Referrals.CannotApplyThisLevel);

            var parentLevel = await levelRepo.GetAsync(parent.LevelId);
            // 申请的等级不能比上级大
            if (level >= parentLevel)
                throw new BusinessException(CrmErrorCodes.Referrals.CannotApplyThisLevel);
        }
    }
}