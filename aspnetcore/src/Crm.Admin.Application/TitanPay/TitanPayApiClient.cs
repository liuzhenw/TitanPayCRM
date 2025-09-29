using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Options;

namespace Crm.Admin.TitanPay;

public class TitanPayApiClient
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new TitanPayTimestampJsonConverter() }
    };

    private readonly HttpClient _httpClient;

    public TitanPayApiClient(HttpClient httpClient, IOptions<TitanPayApiOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("SHOP-ID", options.Value.ShopId);
    }

    public async Task<T> SendAsync<T>(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
        return content!;
    }
}

public record TitanPayApiResponse
{
    public required int Code { get; init; }
    public required string Msg { get; init; }
    public bool IsSuccess => Code == 200;
}

public record TitanPayApiPagedResponse<T> : TitanPayApiResponse
{
    public required List<T> Rows { get; init; }
}

public record TitanPayApiPagedParams
{
    public uint? PageNum { get; init; }
    public uint? PageSize { get; init; }
    public string? OrderByColumn { get; init; }
    public string? IsAsc { get; init; }

    public override string ToString()
    {
        var properties = GetType().GetProperties().Where(x => x.CanRead);
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        foreach (var property in properties)
        {
            var value = property.GetValue(this)?.ToString();
            if (string.IsNullOrWhiteSpace(value)) continue;

            var name = $"{char.ToLowerInvariant(property.Name[0])}{property.Name[1..]}";
            queryString.Add(name, value);
        }

        return queryString.ToString() ?? string.Empty;
    }
}