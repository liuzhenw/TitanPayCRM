using Crm.Settings;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Crm.Admin.BackgroundWorkers;

public class HomeDataUpdateWorker : AsyncPeriodicBackgroundWorkerBase
{
    private static uint _uCardDayIncrement;
    private static DateOnly _periodStartDate = DateOnly.FromDateTime(DateTime.Now);

    public HomeDataUpdateWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60 * 1000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        if (_periodStartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            _periodStartDate = DateOnly.FromDateTime(DateTime.Now);
            _uCardDayIncrement = 0;
        }

        var settingProvider = workerContext.ServiceProvider.GetRequiredService<ISettingProvider>();
        var shouldIncrement = await settingProvider.GetAsync<uint>(CrmSettings.UCardDayIncrement);
        if (_uCardDayIncrement >= shouldIncrement) return;

        var totalSaleVolume = await settingProvider.GetAsync<ulong>(CrmSettings.UCardTotalSaleVolume);
        var minute = 1440 - DateTimeOffset.Now.Hour * 60 + DateTimeOffset.Now.Minute;
        var diff = shouldIncrement - _uCardDayIncrement;
        uint count;
        if (minute >= diff)
        {
            if (Random.Shared.Next(2) == 0) return;

            count = (uint)Random.Shared.Next(1, 3);
        }
        else
        {
            count = (uint)Random.Shared.Next(1, 10);
        }

        _uCardDayIncrement += count;
        totalSaleVolume += count;
        var settingManager = workerContext.ServiceProvider.GetRequiredService<SettingManager>();
        await settingManager.SetGlobalAsync(CrmSettings.UCardTotalSaleVolume, totalSaleVolume.ToString());
    }
}