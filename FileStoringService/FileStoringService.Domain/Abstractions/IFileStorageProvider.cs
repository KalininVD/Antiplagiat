namespace FileStoringService.Domain.Abstractions;

public interface IFileStorageProvider
{
    string SaveFile(string fileName, string content);
}