namespace FileAnalysisService.Application.Services;

public interface IWordCloudClient
{
    byte[]? GenerateWordCloud(string text);
}