using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Astra.EntityFrameworkCore.EntityPropertyConverters;

public class JsonNodeConverter() : ValueConverter<JsonNode, string>(
    p => p.ToJsonString(JsonSerializerOptions.Web),
    s => JsonNode.Parse(s, null, default) ?? new JsonObject(null));

public class JsonNodeComparer() : ValueComparer<JsonNode>(
    (v1, v2) => StructuralComparer.Equals(v1, v2),
    v => StructuralComparer.GetHashCode(v),
    v => JsonNode.Parse(v.ToJsonString(null), null, default)!)
{
    private static readonly JsonNodeStructuralComparer StructuralComparer = new();
}

public class JsonNodeStructuralComparer : IEqualityComparer<JsonNode?>
{
    public bool Equals(JsonNode? x, JsonNode? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return JsonNodesEqual(x, y);
    }

    private static bool JsonNodesEqual(JsonNode x, JsonNode y)
    {
        switch (x)
        {
            case JsonValue xv when y is JsonValue yv:
                return xv.ToJsonString() == yv.ToJsonString();

            case JsonObject xo when y is JsonObject yo:
                if (xo.Count != yo.Count) return false;
                foreach (var kvp in xo)
                {
                    if (!yo.TryGetPropertyValue(kvp.Key, out var yoNode)) return false;
                    if (!JsonNodesEqual(kvp.Value!, yoNode!)) return false;
                }
                return true;

            case JsonArray xa when y is JsonArray ya:
                if (xa.Count != ya.Count) return false;
                for (var i = 0; i < xa.Count; i++)
                    if (!JsonNodesEqual(xa[i]!, ya[i]!)) return false;

                return true;
        }

        return false;
    }

    public int GetHashCode(JsonNode? obj)
    {
        if (obj is null) return 0;

        return obj switch
        {
            JsonValue value => value.ToJsonString().GetHashCode(),

            JsonObject jObj =>
                jObj.OrderBy(kvp => kvp.Key) // 保证 key 顺序一致
                    .Aggregate(17, (hash, kvp) =>
                    {
                        var keyHash = kvp.Key.GetHashCode();
                        var valueHash = GetHashCode(kvp.Value);
                        return HashCode.Combine(hash, keyHash, valueHash);
                    }),

            JsonArray jArr =>
                jArr.Aggregate(17, (hash, item) =>
                    HashCode.Combine(hash, GetHashCode(item))),

            _ => obj.ToJsonString().GetHashCode()
        };
    }
}