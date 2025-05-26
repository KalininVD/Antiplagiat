namespace FileAnalysisService.Application.DTOs;

public record WordCloudDtoOut(Guid FileId, string FilePath, string FileName);