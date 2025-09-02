using AutoMapper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.ObjectMapping;

#pragma warning disable CS8603 // 可能返回 null 引用。

namespace Astra.AutoMapper.Converters;

public class EntityCacheValueConverter<TCache, TDto, TKey>(
    IEntityCache<TCache, TKey> cache,
    IObjectMapper objectMapper) :
    IValueConverter<TKey, TDto>
    where TCache : class
    where TDto : class, new()
{
    public TDto Convert(TKey sourceMember, ResolutionContext context)
    {
        if (sourceMember is null) return null;

        try
        {
            var entityCache = cache.GetAsync(sourceMember).Result;
            return objectMapper.Map<TCache, TDto>(entityCache);
        }
        catch (EntityNotFoundException)
        {
            return null;
        }
    }
}