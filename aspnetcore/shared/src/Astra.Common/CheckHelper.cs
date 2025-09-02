using System.Text.RegularExpressions;

namespace Astra.Common;

public static partial class CheckHelper
{
    /// <summary>
    /// 判断字符串是否为邮件地址格式
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsEmailAddress(string? value)
    {
        return value != null && EmailRegex().IsMatch(value);
    }
    
    /// <summary>
    /// 判断字符串是否为URL
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsUrl(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    [GeneratedRegex(@"^[A-Za-z0-9._%+-]{1,64}@[A-Za-z0-9-]{1,63}(\.[A-Za-z0-9-]{1,63})*\.[A-Za-z]{2,63}$")]
    private static partial Regex EmailRegex();
}