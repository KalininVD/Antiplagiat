namespace FileAnalysisService.Domain.Abstractions;

public interface IWordCloudStorageProvider
{
    string SaveWordCloud(string fileName, byte[] imageData);
}