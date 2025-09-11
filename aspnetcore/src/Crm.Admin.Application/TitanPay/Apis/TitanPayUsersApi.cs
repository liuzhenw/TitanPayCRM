namespace Crm.Admin.TitanPay.Apis;

public static class TitanPayUsersApi
{
    public static async Task<List<TitanPayUser>> GetUsersAsync(this TitanPayApiClient client, TitanPayApiPagedParams? input)
    {
        const string apiUrl = "api/users";
        var url = $"{apiUrl}?{input}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync<TitanPayApiPagedResponse<TitanPayUser>>(request);
        if (!response.IsSuccess)
            throw new HttpRequestException($"{response.Code}: {response.Msg}");
        
        return response.Rows;
    }
}

public record TitanPayUser
{
    public required ulong Id { get; init; }
    public required string Email { get; init; }
    public required string InviteCode { get; init; }
    public required string ReferralUser { get; init; }
    public required DateTimeOffset CreateTime { get; init; }
}