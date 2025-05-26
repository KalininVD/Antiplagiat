namespace FileAnalysisService.Application.DTOs;

public record FileDto(Guid Id, string FileName, string FileContent);