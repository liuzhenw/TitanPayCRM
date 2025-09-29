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
        Log.Information("{Seeder} 初始化完成!", nameof(AppDataSeeder));
    }

    private async Task InitAsync()
    {
        await InitAccountsAsync();
        await InitProductsAsync();
        await InitReferralsAsync();
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
            var card = new Product(CrmConsts.ProductUCard, "TitanPay U 卡", 200);
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
            var team = await referralManager.CreateReferralLevelAsync("agent", "代理", 1, 0.05m);
            var group = await referralManager.CreateReferralLevelAsync("group", "团队长", 2, 0.025m);
            var shareholder = await referralManager.CreateReferralLevelAsync("shareholder", "股东", 3, 0.125m);
            await levelRepo.InsertManyAsync([team, group, shareholder]);
        }
        
        await uow.CompleteAsync();
    }
}