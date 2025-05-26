using System.Security.Cryptography;
using FileStoringService.Domain.Abstractions;

namespace FileStoringService.Domain.Services;

public class FileHashingService : IFileHashingService
{
    public string CalculateHash(string content)
    {
        var hashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(hashBytes);
    }
}