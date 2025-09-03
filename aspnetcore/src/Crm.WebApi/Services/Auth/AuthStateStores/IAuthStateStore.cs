namespace Crm.Services.Auth.AuthStateStores;

public interface IAuthStateStore
{
    Task<AuthState> GenerateAsync(string key);
    Task<AuthState?> FindAsync(string key);
    Task<AuthState> SetAsync(string key, AuthState authState);
}

public class AuthState
{
    public string Code { get; set; } = null!;
    public uint RetryCount { get; set; }
    public DateTimeOffset ExpireAt { get; set; }

    public DateTimeOffset? GenerateAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public bool Verify(string code)
    {
        if (ExpireAt < DateTimeOffset.Now) return false;
        return Code == code;
    }
}