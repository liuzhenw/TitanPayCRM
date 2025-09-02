using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Astra.Localization;

public static class LocalizablePropertyExtensions
{
    public static void Set(this Localizable localizable, string culture, string propertyName, string value)
    {
        if (!localizable.TryGetValue(culture, out var properties))
        {
            properties = new LocalizableProperties();
            localizable[culture] = properties;
        }
        localizable[culture][propertyName] = value;
    }
    
    public static string? GetLocalizableValue(
        this Localizable localizable, string propertyName, string? defaultValue = null)
    {
        if (localizable.Count < 1)
            return defaultValue;

        LocalizableProperties? property = null;
        var culture = CultureInfo.CurrentCulture;
        while (!string.IsNullOrEmpty(culture.Name))
        {
            if (localizable.TryGetValue(culture.Name, out property))
                break;

            culture = culture.Parent;
        }

        if (property is null ||
            property.TryGetValue(propertyName, out var value) is false ||
            value.IsNullOrWhiteSpace())
            return defaultValue;

        return value;
    }
    
    public static string? GetLocalizableValue<T, TProp>(this T entity, Expression<Func<T, TProp>> expression)
        where T : IHasLocalizable
    {
        var propertyName = GetPropertyName(expression);
        var type = typeof(T);
        var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property is null)
            throw new ArgumentException($"找不到属性 {propertyName}");

        var getter = CreatePropertyGetter<T>(property);
        var value = getter(entity);
        return GetLocalizableValue(entity.Localizable, propertyName, value);
    }

    /// <summary>
    /// 创建一个委托，用于从指定类型的实例中获取特定属性的值。
    /// </summary>
    /// <typeparam name="T">包含要访问的属性的类型。</typeparam>
    /// <param name="propertyInfo">要创建getter方法的目标属性信息。</param>
    /// <returns>返回一个委托，该委托接受类型为T的对象作为参数，并返回指定属性的string?值。</returns>
    /// <exception cref="ArgumentException">如果提供的属性不是string类型，则抛出此异常。</exception>
    private static Func<T, string?> CreatePropertyGetter<T>(PropertyInfo propertyInfo)
    {
        // 确保属性类型是 string
        if (propertyInfo.PropertyType != typeof(string))
            throw new ArgumentException($"属性 {propertyInfo.Name} 不是 string 类型");

        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyInfo);
        return Expression.Lambda<Func<T, string?>>(property, param).Compile();
    }

    /// <summary>
    /// 提取属性名；默认返回最内层属性名（不返回字段，不支持索引器/方法）。
    /// 传入嵌套属性时，可选择返回整个路径，如 "Parent.Child.Name"。
    /// </summary>
    private static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> expression, bool fullPath = false)
    {
        ArgumentNullException.ThrowIfNull(expression);

        // 兼容 Convert 包裹（当 TProp 是 object 且属性是值类型时常见）
        var body = expression.Body is UnaryExpression { NodeType: ExpressionType.Convert } u
            ? u.Operand
            : expression.Body;

        // 必须是成员访问
        if (body is not MemberExpression member)
            throw new ArgumentException("表达式不是属性访问", nameof(expression));

        // 验证是“属性”而不是字段
        if (member.Member is not PropertyInfo)
            throw new ArgumentException("仅支持属性，不支持字段", nameof(expression));

        return !fullPath
            ? member.Member.Name // 只要最内层属性名
            : BuildFullPath(member); // 需要完整路径：逐层向外攀爬

        static string BuildFullPath(MemberExpression m)
        {
            // 先拿当前层的名字
            string name = m.Member.Name;

            // 看上层是不是还有 MemberExpression（例如 x.Sub.Prop 的 Sub）
            while (m.Expression is MemberExpression inner)
            {
                if (inner.Member is not PropertyInfo)
                    throw new ArgumentException("路径中包含非属性成员（字段或方法）");

                name = inner.Member.Name + "." + name;
                m = inner;
            }

            // 如果最外层不是参数而是别的，比如方法调用、常量拼装，直接拒绝
            if (m.Expression is not ParameterExpression)
                throw new ArgumentException("不支持的表达式形态（最外层不是参数）");

            return name;
        }
    }
}