using System.Collections.Concurrent;
using System.Reflection;

namespace Astra.Common;

public static class ConstantHelper
{
    private static readonly ConcurrentDictionary<Type, List<string>> Types = new();

    public static List<string> GetAllConstProperties(Type type)
    {
        if (Types.TryGetValue(type, out var properties))
        {
            return properties;
        }
        
        properties = [];
        var fields = type.GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f is { IsLiteral: true, IsInitOnly: false });
        foreach (var filed in fields)
        {
            var value = filed.GetValue(null)?.ToString();
            if (value is null)
                continue;

            properties.Add(value);
        }

        Types.TryAdd(type, properties);
        return properties;
    }
}