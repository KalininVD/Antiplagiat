namespace FileStoringService.Domain.Abstractions;

public interface IFileHashingService
{
    string CalculateHash(string content);
}