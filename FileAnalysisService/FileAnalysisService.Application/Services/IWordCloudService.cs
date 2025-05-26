using FileAnalysisService.Application.DTOs;

namespace FileAnalysisService.Application.Services;

public interface IWordCloudService
{
    WordCloudDtoOut? GetWordCloud(WordCloudDtoIn dto);
}