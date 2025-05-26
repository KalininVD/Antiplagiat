using FileAnalysisService.Application.DTOs;

namespace FileAnalysisService.Application.Services;

public interface IReportService
{
    ReportDtoOut? GetReport(ReportDtoIn dto);
}