using System.Text.Json.Nodes;
using Astra;
using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;
using Serilog;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Crm.DbMigrations.App;

public class AppDataSeeder(IServiceProvider services, IAbpHostEnvironment environment) :
    IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        Log.Information("{Seeder} 开始初始化...", nameof(AppDataSeeder));
        await InitAsync();
        //await MockAsync();
        Log.Information("{Seeder} 初始化完成!", nameof(AppDataSeeder));
    }

    private async Task InitAsync()
    {
        await InitAccountsAsync();
        await InitProductsAsync();
        await InitReferralsAsync();
    }

    private async Task MockAsync()
    {
        if (!environment.IsDevelopment()) return;
        
        await SeedReferralRelationsAsync();
        await SeedProductSaleLogsAsync();
    }

    private async Task InitAccountsAsync()
    {
        await using var scope = services.CreateAsyncScope(); 
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(true);
        
        var roleRepo = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
        var root = await roleRepo.FindAsync(AstraConsts.RootRole);
        if (root is null)
        {
            root = new Role(AstraConsts.RootRole, "Root", true, false);
            await roleRepo.InsertAsync(root, true);
        }

        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
        var rootUser = await userRepo.FindByNameAsync(AstraConsts.RootUser);
        if (rootUser is null)
        {
            rootUser = await userManager.CreateAsync(string.Empty, AstraConsts.RootUser, "P@ssword");
            await userRepo.InsertAsync(rootUser, true);
        }

        await uow.CompleteAsync();
    }

    private async Task InitProductsAsync()
    {
        await using var scope = services.CreateAsyncScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(true);
        
        var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        if (await productRepo.GetCountAsync() < 1)
        {
            var card = new Product(CrmConsts.ProductUCard, "TitanPay U 卡", 100);
            await productRepo.InsertAsync(card, true);
        }
        
        await uow.CompleteAsync();
    }

    private async Task InitReferralsAsync()
    {
        await using var scope = services.CreateAsyncScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(true);
        
        var levelRepo = scope.ServiceProvider.GetRequiredService<IReferrerLevelRepository>();
        if (await levelRepo.GetCountAsync() < 1)
        {
            var referralManager = scope.ServiceProvider.GetRequiredService<ReferralManager>();
            var team = await referralManager.CreateReferralLevelAsync("agent", "推广代理", 1, 0.1m);
            var group = await referralManager.CreateReferralLevelAsync("group", "大团队长", 2, 0.2m);
            var shareholder = await referralManager.CreateReferralLevelAsync("shareholder", "股东", 3, 0.25m);
            await levelRepo.InsertManyAsync([team, group, shareholder]);
        }
        
        await uow.CompleteAsync();
    }


    private async Task SeedReferralRelationsAsync()
    {
        var relationRepo = services.GetRequiredService<IReferralRelationRepository>();
        if (await relationRepo.GetCountAsync() > 0) return;

        KeyValuePair<string?, string>[] relations =
        [
            new("user1@email.com", "user1-1@email.com"),
            new("user1@email.com", "user1-2@email.com"),
            new("user1@email.com", "user1-3@email.com"),
            new("user1@email.com", "user1-4@email.com"),
            new("user1@email.com", "user1-5@email.com"),
            new("user1-1@email.com", "user1-1-1@email.com"),
            new("user1-1@email.com", "user1-1-2@email.com"),
            new("user1-1@email.com", "user1-1-3@email.com"),
            new("user1-1@email.com", "user1-1-4@email.com"),
            new("user1-2@email.com", "user1-2-1@email.com"),
            new("user1-2@email.com", "user1-2-2@email.com"),
            new("user1-1-1@email.com", "user1-1-1-1@email.com"),
            new("user1-1-1@email.com", "user1-1-1-2@email.com"),
            new("user1-1-1@email.com", "user1-1-1-3@email.com"),
            new("user1-2-1@email.com", "user1-2-1-1@email.com"),
            new("user1-2-2@email.com", "user1-2-2-1@email.com"),
            new("user1-1-1-1@email.com", "user1-1-1-1-1@email.com"),
            new("user1-1-1-1@email.com", "user1-1-1-1-2@email.com"),

            new("user2@email.com", "user2-1@email.com"),
            new("user2-1@email.com", "user2-1-1@email.com"),
            new("user2-1@email.com", "user2-1-2@email.com"),
            new("user2-1@email.com", "user2-1-3@email.com"),
            new("user2-1-1@email.com", "user2-1-1-1@email.com"),
            new("user2-1-1@email.com", "user2-1-2-1@email.com"),

            new("user3@email.com", "user3-1@email.com"),
            new("user3@email.com", "user3-2@email.com"),

            new(null, "user4@email.com")
        ];

        var uowManager = services.GetRequiredService<IUnitOfWorkManager>();

        foreach (var relation in relations)
        {
            await using var scope = services.CreateAsyncScope();
            using var uow = uowManager.Begin(true);
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
            var referralManager = scope.ServiceProvider.GetRequiredService<ReferralManager>();

            var recommendee = await userManager.GetOrCreateAsync(relation.Value);
            if (relation.Key is not null)
            {
                var recommender = await userManager.GetOrCreateAsync(relation.Key);
                await referralManager.AddRelationAsync(recommender, recommendee);
            }

            await uow.CompleteAsync();
        }
    }

    private async Task SeedProductSaleLogsAsync()
    {
        var saleLogRepo = services.GetRequiredService<IProductSaleLogRepository>();
        if (await saleLogRepo.GetCountAsync() > 0) return;

        List<string> emails =
        [
            "user1-1@email.com",
            "user1-1@email.com",
            "user1-2@email.com",
            "user1-3@email.com",
            "user1-4@email.com",
            "user1-1-1@email.com",
            "user1-1-2@email.com",
            "user1-1-3@email.com",
            "user1-1-1-1@email.com",
            "user1-1-1-2@email.com",
            "user1-1-1-1-2@email.com",

            "user2@email.com",
            "user2-1@email.com",
            "user2-1-2@email.com",

            "user3-1@email.com"
        ];

        var uowManager = services.GetRequiredService<IUnitOfWorkManager>();
        foreach (var email in emails)
        {
            await using var scope = services.CreateAsyncScope();
            using var uow = uowManager.Begin(true);
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var productManager = scope.ServiceProvider.GetRequiredService<ProductManager>();
            var product = await productRepo.GetAsync(CrmConsts.ProductUCard);
            var user = await userRepo.FindByEmailAsync(email);
            await productManager.SoldAsync(
                product, user!, DateTimeOffset.Now.Ticks.ToString(), 1, new JsonObject(),DateTimeOffset.Now);
            await uow.CompleteAsync();
        }
    }
}