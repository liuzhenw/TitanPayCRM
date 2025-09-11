using System.Text.Json;
using System.Text.Json.Serialization;

namespace Crm.Admin.TitanPay;

public class TitanPayTimestampJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()),
            JsonTokenType.String => DateTimeOffset.FromUnixTimeSeconds(long.Parse(reader.GetString() ?? string.Empty)),
            _ => throw new JsonException($"无法将 {reader.TokenType} 值转换为 {nameof(DateTimeOffset)}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.ToUnixTimeSeconds());
    }
}