using Crm.Settings;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace Crm.Controllers;

[ApiController]
[Route("home")]
public class HomeController(ISettingProvider settingProvider) : CrmController
{
    /// <summary>
    /// 获取主页数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<HomeViewModel> GetAsync()
    {
        var totalSaleVolume = await settingProvider.GetAsync<ulong>(CrmSettings.UCardTotalSaleVolume);
        return new HomeViewModel(totalSaleVolume);
    }
}

public record HomeViewModel(ulong UCardTotalSaleVolume);