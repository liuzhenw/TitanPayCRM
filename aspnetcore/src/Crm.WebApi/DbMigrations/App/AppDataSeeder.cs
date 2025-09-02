using Astra;
using Crm.Accounts;
using Crm.Products;
using Crm.Referrals;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Crm.DbMigrations.App;

public class AppDataSeeder(IServiceProvider services) : IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        await InitAccountsAsync();
        await InitProductsAsync();
        await InitReferralsAsync();
    }

    private async Task InitAccountsAsync()
    {
        await using var scope = services.CreateAsyncScope();
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
            rootUser = await userManager.CreateAsync("root@email.com", AstraConsts.RootUser, "P@ssword");
            await userRepo.InsertAsync(rootUser, true);
        }
    }

    private async Task InitProductsAsync()
    {
        await using var scope = services.CreateAsyncScope();
        var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        if (await productRepo.GetCountAsync() < 1)
        {
            var card = new Product("u_card", "TitanPay U 卡", 100);
            await productRepo.InsertAsync(card, true);
        }
    }

    private async Task InitReferralsAsync()
    {
        await using var scope = services.CreateAsyncScope();
        var levelRepo = scope.ServiceProvider.GetRequiredService<IReferrerLevelRepository>();
        if (await levelRepo.GetCountAsync() < 1)
        {
            var referralManager = scope.ServiceProvider.GetRequiredService<ReferralManager>();
            var team = await referralManager.CreateReferralLevelAsync("team", "团队长", 1, 0.1m);
            var group = await referralManager.CreateReferralLevelAsync("group", "大团队长", 2, 0.2m);
            var shareholder = await referralManager.CreateReferralLevelAsync("shareholder", "股东", 3, 0.25m);
            await levelRepo.InsertManyAsync([team, group, shareholder]);
        }
    }
}