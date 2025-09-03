using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Crm.Services.Auth.AuthStateStores;

public class MemoryAuthStateStore(IMemoryCache cache) : IAuthStateStore, ITransientDependency
{
    public Task<AuthState?> FindAsync(string key)
    {
        key = key.ToLowerInvariant();
        return Task.FromResult(cache.Get<AuthState>(key));
    }

    public Task<AuthState> SetAsync(string key, AuthState state)
    {
        key = key.ToLowerInvariant();
        var cacheExpireAt = TimeSpan.FromDays(1) - (DateTimeOffset.Now - state.CreatedAt);
        if (cacheExpireAt <= TimeSpan.Zero) cacheExpireAt = TimeSpan.FromDays(1);
        cache.Set(key, state, cacheExpireAt);
        return Task.FromResult(state);
    }

    public async Task<AuthState> GenerateAsync(string key)
    {
        var state = await FindAsync(key);
        if (DateTimeOffset.Now - state?.GenerateAt < TimeSpan.FromMinutes(1))
            throw new BusinessException(CrmErrorCodes.Accounts.SendEmailTooFast);
        
        if (state?.RetryCount > 10)
            throw new BusinessException(CrmErrorCodes.Accounts.SendEmailTooMany);

        state ??= new AuthState();
        state.Code = GenerateCode();
        state.GenerateAt = DateTimeOffset.Now;
        state.ExpireAt = DateTimeOffset.Now.AddMinutes(30);
        state.RetryCount++;
        await SetAsync(key, state);
        return state;
    }

    private static string GenerateCode()
    {
        return Random.Shared.Next(100000, 999999).ToString();
    }
}