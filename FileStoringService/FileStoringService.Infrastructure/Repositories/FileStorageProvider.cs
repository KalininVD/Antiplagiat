using FileStoringService.Domain.Abstractions;

namespace FileStoringService.Infrastructure.Repositories;

public class FileStorageProvider(string basePath) : IFileStorageProvider
{
    private readonly string _basePath = basePath;

    public string SaveFile(string fileName, string content)
    {
        var fileExtension = Path.GetExtension(fileName);

        if (string.IsNullOrEmpty(fileExtension) || fileExtension != ".txt")
        {
            fileName = Path.GetFileNameWithoutExtension(fileName) + ".txt";
        }
        
        fileName = DateTime.UtcNow.ToString("ddMMyyyy_HHmmss") + "_" + fileName;
        var filePath = Path.Combine(_basePath, fileName);

        File.WriteAllText(filePath, content);

        return filePath;
    }
}