using System.Text.Json.Serialization;

namespace Crm.Admin.TitanPay.Apis;

public static class TitanPayCardsApi
{
    public static async Task<List<TitanPayCard>> GetCardsAsync(this TitanPayApiClient client, TitanPayApiPagedParams input)
    {
        const string apiUrl = "api/cards";
        var url = $"{apiUrl}?{input}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync<TitanPayApiPagedResponse<TitanPayCard>>(request);
        if (!response.IsSuccess)
            throw new HttpRequestException($"{response.Code}: {response.Msg}");
        
        return response.Rows;
    }
}

public record TitanPayCard
{
    public required ulong Id { get; init; }
    public required string Email { get; init; }
    public required string CardType { get; init; }
    [JsonPropertyName("cardNo")]
    public required string CardNo { get; init; }
    public required DateTimeOffset CreateTime { get; init; }
}