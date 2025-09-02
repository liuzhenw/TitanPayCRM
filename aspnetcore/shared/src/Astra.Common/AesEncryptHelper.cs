using System.Security.Cryptography;
using System.Text;

namespace Astra.Common;

public static class AesEncryptHelper
{
    public const int IVSize = 16;
    public const int KeySize = 32;
    public const int NonceSize = 12;
    public const int TagSize = 16;

    private static byte[] DeriveKeyWithSha256(string source, uint size)
    {
        var result = new byte[size];
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(source));
        var index = 0;
        while (index < result.Length)
        {
            bytes = SHA256.HashData(bytes);
            var remaining = result.Length - index;
            var length = bytes.Length > remaining ? remaining : bytes.Length;
            Array.Copy(bytes, 0, result, index, length);
            index += length;
        }

        return result;
    }

    /// <summary>
    /// CBC 模式加密
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] EncryptCBC(byte[] plaintext, string key)
    {
        var iv = new byte[IVSize];
        RandomNumberGenerator.Fill(iv);
        using var aes = Aes.Create();
        aes.Key = DeriveKeyWithSha256(key, KeySize);
        var ciphertext = aes.EncryptCbc(plaintext, iv);
        var result = new byte[iv.Length + ciphertext.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(ciphertext, 0, result, iv.Length, ciphertext.Length);
        return result;
    }

    /// <summary>
    /// CBC 模式解密
    /// </summary>
    /// <param name="encryptedBytes"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] DecryptCBC(byte[] encryptedBytes, string key)
    {
        var iv = encryptedBytes[..IVSize];
        var ciphertext = encryptedBytes[IVSize..];
        using var aes = Aes.Create();
        aes.Key = DeriveKeyWithSha256(key, KeySize);
        return aes.DecryptCbc(ciphertext, iv);
    }

    /// <summary>
    /// GCM 模式加密
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] EncryptGCM(byte[] plaintext, string key)
    {
        var keyBytes = DeriveKeyWithSha256(key, KeySize);
        var nonceBytes = new byte[NonceSize];
        RandomNumberGenerator.Fill(nonceBytes);
        using var aes = new AesGcm(keyBytes, TagSize);
        var tagBytes = new byte[TagSize];
        var ciphertext = new byte[plaintext.Length];
        aes.Encrypt(nonceBytes, plaintext, ciphertext, tagBytes);
        var result = new byte[nonceBytes.Length + ciphertext.Length + tagBytes.Length];
        Buffer.BlockCopy(nonceBytes, 0, result, 0, nonceBytes.Length);
        Buffer.BlockCopy(ciphertext, 0, result, nonceBytes.Length, ciphertext.Length);
        Buffer.BlockCopy(tagBytes, 0, result, nonceBytes.Length + ciphertext.Length, tagBytes.Length);
        return result;
    }

    /// <summary>
    /// GCM 模式解密
    /// </summary>
    /// <param name="encryptedBytes"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static byte[] DecryptGCM(byte[] encryptedBytes, string key)
    {
        var keyBytes = DeriveKeyWithSha256(key, KeySize);
        var nonceBytes = encryptedBytes[..NonceSize];
        var ciphertext = encryptedBytes[NonceSize..^TagSize];
        var tagBytes = encryptedBytes[^TagSize..];
        using var aes = new AesGcm(keyBytes, TagSize);
        var plaintext = new byte[ciphertext.Length];
        aes.Decrypt(nonceBytes, ciphertext, tagBytes, plaintext);
        return plaintext;
    }

    /// <summary>
    /// CBC 模式加密字符串
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string EncryptCBC(string plaintext, string key)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = EncryptCBC(bytes, key);
        return Convert.ToBase64String(ciphertext);
    }

    /// <summary>
    /// CBC 模式解密字符串
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string DecryptCBC(string ciphertext, string key)
    {
        var bytes = Convert.FromBase64String(ciphertext);
        var plaintext = DecryptCBC(bytes, key);
        return Encoding.UTF8.GetString(plaintext);
    }

    /// <summary>
    /// GCM 模式加密字符串
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string EncryptGCM(string plaintext, string key)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = EncryptGCM(bytes, key);
        return Convert.ToBase64String(ciphertext);
    }

    /// <summary>
    /// GCM 模式解密字符串
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string DecryptGCM(string ciphertext, string key)
    {
        var bytes = Convert.FromBase64String(ciphertext);
        var plaintext = DecryptGCM(bytes, key);
        return Encoding.UTF8.GetString(plaintext);
    }
}