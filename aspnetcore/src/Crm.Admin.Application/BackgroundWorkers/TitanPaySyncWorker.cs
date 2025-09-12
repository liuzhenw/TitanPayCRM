using System.Text.Json.Nodes;
using Crm.Accounts;
using Crm.Admin.TitanPay;
using Crm.Admin.TitanPay.Apis;
using Crm.Products;
using Crm.Referrals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Crm.Admin.BackgroundWorkers;

public class TitanPaySyncWorker : AsyncPeriodicBackgroundWorkerBase
{
    private IServiceProvider _services = null!;

    public TitanPaySyncWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 1 * 60 * 1000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        _services = workerContext.ServiceProvider;
        await SyncTitanPayUsersAsync(workerContext.CancellationToken);
        await SyncTitanPayCardsAsync(workerContext.CancellationToken);
    }

    private async Task SyncTitanPayUsersAsync(CancellationToken ct)
    {
        Logger.LogDebug("开始同步 TitanPay 用户数据...");
        var userRepo = _services.GetRequiredService<IUserRepository>();
        var relationRepo = _services.GetRequiredService<IReferralRelationRepository>();
        var userManager = _services.GetRequiredService<UserManager>();
        var referralManager = _services.GetRequiredService<ReferralManager>();

        var lastUser = await userRepo.FindLastAsync();
        var client = _services.GetRequiredService<TitanPayApiClient>();
        var pageNum = 1u;
        var pageSize = 100u;
        List<TitanPayUser> addUsers = [];
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var input = new TitanPayApiPagedParams
            {
                IsAsc = "desc",
                OrderByColumn = "userId",
                PageNum = pageNum,
                PageSize = pageSize
            };

            Logger.LogDebug("正在获取第 {PageNum} 页用户...", pageNum);
            var users = await client.GetUsersAsync(input);
            if (users.Count < 1) break;

            if (lastUser is not null && lastUser.CreatedAt > users.Last().CreateTime)
            {
                addUsers.AddRange(users
                    .Where(x => x.CreateTime >= lastUser.CreatedAt)
                    .OrderBy(x => x.CreateTime));
                break;
            }

            addUsers.AddRange(users);
            pageNum++;
        }

        var count = 0;
        foreach (var titanPayUser in addUsers.OrderBy(x => x.Id))
        {
            User? recommender = null;
            if (!titanPayUser.ReferralUser.IsNullOrWhiteSpace())
                recommender = await userRepo.FindByEmailAsync(titanPayUser.ReferralUser)
                              ?? await userManager.ImportAsync(titanPayUser.ReferralUser, titanPayUser.CreateTime);

            var recommendee = await userRepo.FindByEmailAsync(titanPayUser.Email)
                              ?? await userManager.ImportAsync(titanPayUser.Email, titanPayUser.CreateTime);

            if (recommender is not null)
            {
                if (await relationRepo.ExistsAsync(recommendee.Id))
                    continue;

                await referralManager.AddRelationAsync(recommender, recommendee);
            }

            count++;
        }

        Logger.LogDebug("同步 TitanPay 用户数据完成, 新增 {UserCount} 个用户!", count);
    }

    private async Task SyncTitanPayCardsAsync(CancellationToken ct)
    {
        Logger.LogDebug("开始同步 TitanPay U 卡数据...");
        var userRepo = _services.GetRequiredService<IUserRepository>();
        var productRepo = _services.GetRequiredService<IProductRepository>();
        var saleLogRepo = _services.GetRequiredService<IProductSaleLogRepository>();
        var productManager = _services.GetRequiredService<ProductManager>();

        var product = await productRepo.FindAsync(CrmConsts.ProductUCard, cancellationToken: ct);
        if (product is null) return;
        
        var lastSaleLog = await saleLogRepo.FindLastAsync(product.Id);
        var client = _services.GetRequiredService<TitanPayApiClient>();
        var pageNum = 1u;
        var pageSize = 100u;
        List<TitanPayCard> addCards = [];
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var input = new TitanPayApiPagedParams
            {
                IsAsc = "desc",
                OrderByColumn = "id",
                PageNum = pageNum,
                PageSize = pageSize
            };
            Logger.LogDebug("正在获取第 {PageNum} 页 U 卡...", pageNum);
            var cards = await client.GetCardsAsync(input);
            if (cards.Count < 1) break;

            if (lastSaleLog is not null && lastSaleLog.CreatedAt > cards.Last().CreateTime)
            {
                addCards.AddRange(cards
                    .Where(x => x.CreateTime >= lastSaleLog.CreatedAt)
                    .OrderBy(x => x.CreateTime));
                break;
            }

            addCards.AddRange(cards);
            pageNum++;
        }

        var count = 0;
        foreach (var titanPayCard in addCards.Where(x => x.CardType == "ENTITY").OrderBy(x => x.Id))
        {
            if (await saleLogRepo.ExistsAsync(product.Id, titanPayCard.Id.ToString()))
                continue;

            var user = await userRepo.FindByEmailAsync(titanPayCard.Email);
            var data = new JsonObject
            {
                { "Id", titanPayCard.Id },
                { "CardType", titanPayCard.CardType },
                { "CardNo", titanPayCard.CardNo }
            };
            await productManager.SoldAsync(
                product, user!, titanPayCard.Id.ToString(), 1, data, titanPayCard.CreateTime);
            count++;
        }

        Logger.LogDebug("同步 TitanPay U 卡数据完成, 新增 {Count} 张 U 卡!", count);
    }
}