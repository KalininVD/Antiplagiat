using FileAnalysisService.Domain.Abstractions;

namespace FileAnalysisService.Infrastructure.Repositories;

public class WordCloudStorageProvider(string basePath) : IWordCloudStorageProvider
{
    private readonly string _basePath = basePath;

    public string SaveWordCloud(string fileName, byte[] imageData)
    {
        var fileExtension = Path.GetExtension(fileName);

        if (string.IsNullOrEmpty(fileExtension) || fileExtension != ".png")
        {
            fileName = Path.GetFileNameWithoutExtension(fileName) + ".png";
        }

        fileName = DateTime.UtcNow.ToString("ddMMyyyy_HHmmss") + "_" + fileName;
        var filePath = Path.Combine(_basePath, fileName);

        File.WriteAllBytes(filePath, imageData);

        return filePath;
    }
}