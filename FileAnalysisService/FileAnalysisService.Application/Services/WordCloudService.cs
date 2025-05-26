using Microsoft.Extensions.DependencyInjection;
using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Application.DTOs;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Services;

public class WordCloudService(IServiceScopeFactory serviceScopeFactory) : IWordCloudService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public WordCloudDtoOut? GetWordCloud(WordCloudDtoIn dto)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var wordCloudRepository = scope.ServiceProvider.GetRequiredService<IWordCloudRepository>();

            var wordCloud = wordCloudRepository.GetById(dto.FileId);
            if (wordCloud == null)
            {
                return CreateWordCloud(dto);
            }

            return new WordCloudDtoOut(
                wordCloud.FileId,
                wordCloud.FilePath,
                wordCloud.FileName
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public WordCloudDtoOut? CreateWordCloud(WordCloudDtoIn dto)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var wordCloudRepository = scope.ServiceProvider.GetRequiredService<IWordCloudRepository>();
        var fileStoringClient = scope.ServiceProvider.GetRequiredService<IFileStoringClient>();
        var wordCloudClient = scope.ServiceProvider.GetRequiredService<IWordCloudClient>();
        var wordCloudStorageProvider = scope.ServiceProvider.GetRequiredService<IWordCloudStorageProvider>();

        var fileDto = fileStoringClient.GetFileById(dto.FileId);
        if (fileDto == null)
        {
            return null;
        }

        var fileName = fileDto.FileName;
        var content = fileDto.FileContent;

        var wordCloudRaw = wordCloudClient.GenerateWordCloud(content);
        if (wordCloudRaw == null)
        {
            return null;
        }

        var filePath = wordCloudStorageProvider.SaveWordCloud(fileName, wordCloudRaw);

        var wordCloud = new WordCloud(
            fileDto.Id,
            filePath,
            Path.GetFileName(filePath)
        );

        wordCloudRepository.Add(wordCloud);

        return new WordCloudDtoOut(
            wordCloud.FileId,
            wordCloud.FilePath,
            wordCloud.FileName
        );
    }
}