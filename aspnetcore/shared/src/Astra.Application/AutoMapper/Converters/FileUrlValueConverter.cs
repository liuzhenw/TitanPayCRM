using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Astra.AutoMapper.Converters;

public partial class FileUrlValueConverter(IConfiguration configuration) : IValueConverter<string?, string?>
{
    public string? Convert(string? sourceMember, ResolutionContext context)
    {
        if (sourceMember.IsNullOrWhiteSpace())
            return null;

        if (UrlRegex().IsMatch(sourceMember))
            return sourceMember;
        
        var baseUrl = configuration["BlobStorage:Endpoint"];
        if (baseUrl.IsNullOrWhiteSpace())
            baseUrl = configuration["App:SelfUrl"] ?? string.Empty;
        return $"{baseUrl}/files/{sourceMember}";
    }

    [GeneratedRegex("^(http|https|ftp)://")]
    private static partial Regex UrlRegex();
}