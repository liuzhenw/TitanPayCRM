using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Crm.DbMigrations.Abp;

public class AbpDataSeeder : IDataSeedContributor, ITransientDependency
{
    public Task SeedAsync(DataSeedContext context)
    {
        return Task.CompletedTask;
    }
    
}