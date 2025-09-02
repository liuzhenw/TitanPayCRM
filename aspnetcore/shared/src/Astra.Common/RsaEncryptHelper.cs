using System.Security.Cryptography;
using System.Text;

namespace Astra.Common;

public static class RsaEncryptHelper
{
    private const string Pkcs1PrivateKeyPemPrefix = "-----BEGIN RSA PRIVATE KEY-----";
    private const string Pkcs1PrivateKeyPemSuffix = "-----END RSA PRIVATE KEY-----";
    private const string Pkcs1PublicKeyPemPrefix = "-----BEGIN RSA PUBLIC KEY-----";
    private const string Pkcs1PublicKeyPemSuffix = "-----END RSA PUBLIC KEY-----";

    private const char SliceFlag = '#';

    private static string Pkcs1PrivateKeyToPem(string privateKey)
    {
        return new StringBuilder()
            .AppendLine(Pkcs1PrivateKeyPemPrefix)
            .Append(ConvertPemByPkcs1(privateKey))
            .Append(Pkcs1PrivateKeyPemSuffix)
            .ToString();
    }

    private static string Pkcs1PublicKeyToPem(string publicKey)
    {
        return new StringBuilder()
            .AppendLine(Pkcs1PublicKeyPemPrefix)
            .Append(ConvertPemByPkcs1(publicKey))
            .Append(Pkcs1PublicKeyPemSuffix)
            .ToString();
    }

    private static string PemToKey(string pem)
    {
        return pem
            .Replace(Pkcs1PrivateKeyPemPrefix, string.Empty)
            .Replace(Pkcs1PrivateKeyPemSuffix, string.Empty)
            .Replace(Pkcs1PublicKeyPemPrefix, string.Empty)
            .Replace(Pkcs1PublicKeyPemSuffix, string.Empty)
            .Replace(Environment.NewLine, string.Empty);
    }

    private static string ConvertPemByPkcs1(string key)
    {
        var sb = new StringBuilder();
        var count = Convert.ToInt32(Math.Ceiling(key.Length * 1.0 / 64));
        for (var i = 0; i < count; i++)
        {
            var start = i * 64;
            var end = start + 64;
            if (end >= key.Length)
            {
                end = key.Length;
            }

            sb.AppendLine(key[start..end]);
        }

        return sb.ToString();
    }

    private static string Encrypt(this RSA rsa, byte[] bytes)
    {
        var result = rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(result);
    }

    private static string Decrypt(this RSA rsa, byte[] bytes)
    {
        var result = rsa.Decrypt(bytes, RSAEncryptionPadding.Pkcs1);
        return Encoding.UTF8.GetString(result);
    }

    /// <summary>
    /// 生成RSA的PKCS#1格式的pem证书
    /// </summary>
    /// <returns></returns>
    public static PemCertificate GenerateRsaPkcs1FormatPemCertificate()
    {
        using var rsa = RSA.Create();
        var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        var key = new PemCertificate(Pkcs1PrivateKeyToPem(privateKey), Pkcs1PublicKeyToPem(publicKey));
        return key;
    }

    /// <summary>
    /// 加密（无法加密过大的数据，大数据加密使用SliceEncrypt）
    /// </summary>
    /// <param name="data">原始数据</param>
    /// <param name="pubKeyPem">pem格式的公钥</param>
    /// <returns></returns>
    public static string Encrypt(string data, string pubKeyPem)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(PemToKey(pubKeyPem)), out _);
        return rsa.Encrypt(Encoding.UTF8.GetBytes(data));
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="encrypt">密文</param>
    /// <param name="priKeyPem">pem格式的私钥</param>
    /// <returns></returns>
    public static string Decrypt(string encrypt, string priKeyPem)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(PemToKey(priKeyPem)), out _);
        return rsa.Decrypt(Convert.FromBase64String(encrypt));
    }

    /// <summary>
    /// 验证证书
    /// </summary>
    /// <param name="pem"></param>
    /// <returns></returns>
    public static bool Verify(PemCertificate pem)
    {
        const string raw = "rsa encrypt test";
        var encrypt = Encrypt(raw, pem.PublicKey);
        try
        {
            var decrypt = Decrypt(encrypt, pem.PrivateKey);
            return decrypt == raw;
        }
        catch (CryptographicException)
        {
            return false;
        }
    }

    /// <summary>
    /// 分片加密，用于大数据加密，数据越大，密文越大
    /// </summary>
    /// <param name="data">原始数据</param>
    /// <param name="pemPubKey">pem格式的公钥</param>
    /// <returns></returns>
    public static string SliceEncrypt(string data, string pemPubKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(PemToKey(pemPubKey)), out _);

        // pkcs1算法 最大加密长度 = key的大小 / 8 - 11                          
        var maxBytesLength = rsa.KeySize / 8 - 11;
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var sliceCount = Convert.ToInt32(Math.Ceiling(dataBytes.Length * 1.0 / maxBytesLength));
        var sb = new StringBuilder();
        for (var i = 0; i < sliceCount; i++)
        {
            var bytes = dataBytes
                .Skip(i * maxBytesLength)
                .Take(maxBytesLength)
                .ToArray();
            sb.Append(rsa.Encrypt(bytes));
            sb.Append(SliceFlag);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 分片解密，用于解密使用分片加密的密文
    /// </summary>
    /// <param name="encrypt">密文</param>
    /// <param name="pemPriKey">pem格式的私钥</param>
    /// <returns></returns>
    public static string SliceDecrypt(string encrypt, string pemPriKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(PemToKey(pemPriKey)), out _);

        var arr = encrypt.Split(SliceFlag, StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach (var item in arr)
        {
            var bytes = Convert.FromBase64String(item);
            sb.Append(rsa.Decrypt(bytes));
        }

        return sb.ToString();
    }
}

public class PemCertificate(string privateKey, string publicKey)
{
    public string PrivateKey { get; } = privateKey;

    public string PublicKey { get; } = publicKey;
}