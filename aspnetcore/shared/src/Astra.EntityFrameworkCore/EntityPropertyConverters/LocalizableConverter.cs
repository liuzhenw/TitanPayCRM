using System.Text.Encodings.Web;
using System.Text.Json;
using Astra.Localization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Astra.EntityFrameworkCore.EntityPropertyConverters;

public class LocalizableConverter() : ValueConverter<Localizable, string>(
    v => JsonSerializer.Serialize(v, JsonOptions),
    v => JsonSerializer.Deserialize<Localizable>(v, JsonOptions) ?? new Localizable())
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerOptions.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}

public class LocalizableComparer() : ValueComparer<Localizable>(
    (v1, v2) => v1 == null ? v2 == null : v1.Equals(v2),
    v => v.GetHashCode(),
    v => (v.Clone() as Localizable)!);