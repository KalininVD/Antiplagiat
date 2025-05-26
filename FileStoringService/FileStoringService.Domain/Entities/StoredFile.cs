namespace FileStoringService.Domain.Entities;

public class StoredFile(string fileName, string fileHash, string filePath)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FileName { get; init; } = fileName;
    public string FileHash { get; init; } = fileHash;
    public string FilePath { get; init; } = filePath;
}