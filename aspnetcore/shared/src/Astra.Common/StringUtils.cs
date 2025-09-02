using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Astra.Common;

public static partial class StringUtils
{
    public static TimeSpan ToTimeSpan(this string? value, TimeSpan defaultValue)
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        return TimeSpan.TryParse(value, out var timeSpan) ? timeSpan : defaultValue;
    }

    /// <summary>
    /// 计算MD5哈希值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ComputeMd5(this string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hashBytes = MD5.HashData(bytes);
        // 将哈希值转换为字符串
        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            // 使用小写字母表示每个字节的值
            sb.Append(t.ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 计算SHA256哈希值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ComputeSha256(this string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hashBytes = SHA256.HashData(bytes);
        // 将哈希值转换为字符串
        var sb = new StringBuilder(hashBytes.Length * 2);
        foreach (var b in hashBytes)
        {
            // 使用小写字母表示每个字节的值
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
    
    /// <summary>
    /// 获取随机字符串（a-zA-Z0-9)
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string RandomString(int length)
    {
        if (length < 1)
        {
            return string.Empty;
        }

        var chars = new[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z',
        };
        var result = new StringBuilder();
        while (length > 0)
        {
            var index = RandomNumberGenerator.GetInt32(chars.Length);
            result.Append(chars[index]);
            length--;
        }

        return result.ToString();
    }

    /// <summary>
    /// 比较两个字符串是否相等，忽略大小写。
    /// </summary>
    /// <param name="value">要比较的第一个字符串。</param>
    /// <param name="other">要比较的第二个字符串。</param>
    /// <returns>如果两个字符串相等（忽略大小写）则返回true，否则返回false。</returns>
    public static bool EqualsIgnoreCase(this string? value, string? other)
    {
        return value switch
        {
            null when other is null => true,
            null => false,
            _ => value.Equals(other, StringComparison.OrdinalIgnoreCase)
        };
    }
}