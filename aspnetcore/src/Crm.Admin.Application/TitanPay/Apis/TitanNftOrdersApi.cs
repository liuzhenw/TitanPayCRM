namespace Crm.Admin.TitanPay.Apis;

public static class TitanNftOrdersApi
{
    public static async Task<List<TitanNftOrder>> GetNftOrdersAsync(this TitanPayApiClient client, TitanPayApiPagedParams input)
    {
        const string apiUrl = "api/nft-orders";
        var url = $"{apiUrl}?{input}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync<TitanPayApiPagedResponse<TitanNftOrder>>(request);
        if (!response.IsSuccess)
            throw new HttpRequestException($"{response.Code}: {response.Msg}");
        
        return response.Rows;
    }
}

public record TitanNftOrder
{
    public required long Id { get; init; }
    public required string Email { get; init; }
    public required long ProjectId { get; init; }
    public string? ProjectName { get; init; }
    public decimal? Amount { get; init; }
    public required DateTimeOffset CreateTime { get; init; }
}