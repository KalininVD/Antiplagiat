using FileAnalysisService.Application.DTOs;

namespace FileAnalysisService.Application.Services;

public interface IFileStoringClient
{
    FileDto? GetFileById(Guid fileId);
}