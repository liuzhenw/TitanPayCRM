using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.ObjectMapping;

#pragma warning disable CS8603 // 可能返回 null 引用。

namespace Astra.AutoMapper.Converters;

public class EntityCacheValueConverter<TCache, TDto, TKey>(IServiceProvider services) : IValueConverter<TKey, TDto>
    where TCache : class
    where TDto : class, new()
{
    private readonly IObjectMapper _objectMapper = services.GetRequiredService<IObjectMapper>();
    private readonly IEntityCache<TCache, TKey> _cache = services.GetRequiredService<IEntityCache<TCache, TKey>>();
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter = services.GetRequiredService<IDataFilter<ISoftDelete>>();

    public TDto Convert(TKey sourceMember, ResolutionContext context)
    {
        if (sourceMember is null) return null;

        using var _ = _softDeleteFilter.Disable();
        try
        {
            var entityCache = _cache.GetAsync(sourceMember).Result;
            return _objectMapper.Map<TCache, TDto>(entityCache);
        }
        catch (EntityNotFoundException)
        {
            return null;
        }
    }
}