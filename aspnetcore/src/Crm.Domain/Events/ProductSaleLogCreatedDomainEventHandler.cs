using System.Threading.Tasks;
using Crm.Products;
using Crm.Referrals;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Crm.Events;

public class ProductSaleLogCreatedDomainEventHandler(ReferralManager manager) :
    ILocalEventHandler<EntityCreatedEventData<ProductSaleLog>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityCreatedEventData<ProductSaleLog> eventData)
    {
        await manager.OnProductSoldAsync(eventData.Entity);
    }
}