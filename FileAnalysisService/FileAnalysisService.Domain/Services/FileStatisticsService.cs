using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Services;

public class FileStatisticsService : IFileStatisticsService
{
    public FileStatistics CalculateStatistics(string content)
    {
        int paragraphCount = content.Split(["\r\n", "\n"], StringSplitOptions.None).Length;
        int totalWordCount = content.Split([' ', '\r', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries).Length;
        int totalSymbolCount = content.Length;

        return new FileStatistics(paragraphCount, totalWordCount, totalSymbolCount);
    }
}