using System.Threading.Tasks;
using Crm.Products;
using Crm.Referrals;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Crm.Events;

public class ProductSaleLogCreatedDomainEventHandler(
    ILogger<ProductSaleLogCreatedDomainEventHandler> logger,
    ReferralManager manager) :
    ILocalEventHandler<EntityCreatedEventData<ProductSaleLog>>,
    ITransientDependency
{
    public async Task HandleEventAsync(EntityCreatedEventData<ProductSaleLog> eventData)
    {
        await manager.OnProductSoldAsync(eventData.Entity);
        logger.LogDebug(
            "售出 {Quantity} 件 {ProductId} 商品",
            eventData.Entity.Quantity, eventData.Entity.ProductId);
    }
}