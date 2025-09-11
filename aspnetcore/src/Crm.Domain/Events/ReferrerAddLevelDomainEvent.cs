using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Products;
using Crm.Referrals;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Crm.Events;

public record ReferrerAddLevelDomainEvent(Referrer Referrer);

public class ReferrerAddLevelDomainEventHandler(
    IProductRepository productRepo,
    IProductSaleLogRepository saleLogRepo,
    ICommissionLogRepository commissionLogRepo,
    IReferrerLevelRepository levelRepo) : 
    ILocalEventHandler<ReferrerAddLevelDomainEvent>, ITransientDependency
{
    public async Task HandleEventAsync(ReferrerAddLevelDomainEvent eventData)
    {
        if (eventData.Referrer.LevelId is null) return;
        
        var commissions = await commissionLogRepo.GetZeroCommissionListAsync(eventData.Referrer.Id);
        if (commissions.Count < 1) return;

        var level = await levelRepo.GetAsync(eventData.Referrer.LevelId);
        foreach (var commissionLog in commissions)
        {
            var saleLog = await saleLogRepo.GetAsync(commissionLog.SaleLogId);
            var commission = level.Multiplier * saleLog.Amount;
            commissionLog.OnGrant(commission, level.Id);
            eventData.Referrer.OnCommissionAdded(saleLog,commission);
            saleLog.OnCommissionAdded(commission);
            level.OnCommissionAdded(commission);
        }
        await commissionLogRepo.UpdateManyAsync(commissions);
        await levelRepo.UpdateAsync(level);
        await productRepo.UpdateStatisticAsync();
    }
}