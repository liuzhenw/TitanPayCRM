namespace Astra.Common;

public static class TypeUtils
{
    /// <summary>
    /// 判断是否为可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullable(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// 判断是否为 Enum 可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullableEnum(this Type type)
    {
        return type.IsEnum && type.IsNullable();
    }
}