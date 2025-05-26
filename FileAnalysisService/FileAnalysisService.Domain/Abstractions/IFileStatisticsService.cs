using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Abstractions;

public interface IFileStatisticsService
{
    FileStatistics CalculateStatistics(string content);
}