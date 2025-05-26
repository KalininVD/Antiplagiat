using Microsoft.Extensions.DependencyInjection;
using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Application.DTOs;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Services;

public class ReportService(IServiceScopeFactory serviceScopeFactory) : IReportService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public ReportDtoOut? GetReport(ReportDtoIn dto)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>();

            var report = reportRepository.GetById(dto.FileId);
            if (report == null)
            {
                return CreateNewReport(dto);
            }

            return new ReportDtoOut(
                report.FileId,
                report.FileName,
                report.ParagraphCount,
                report.TotalWordCount,
                report.TotalSymbolCount
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    private ReportDtoOut? CreateNewReport(ReportDtoIn dto)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var reportRepository = scope.ServiceProvider.GetRequiredService<IReportRepository>();
        var fileStoringClient = scope.ServiceProvider.GetRequiredService<IFileStoringClient>();
        var fileStatisticsService = scope.ServiceProvider.GetRequiredService<IFileStatisticsService>();

        var fileDto = fileStoringClient.GetFileById(dto.FileId);
        if (fileDto == null)
        {
            return null;
        }

        var fileName = fileDto.FileName;
        var content = fileDto.FileContent;

        var statistics = fileStatisticsService.CalculateStatistics(content);

        var report = new AnalyticalReport(
            fileDto.Id,
            fileName,
            statistics.ParagraphCount,
            statistics.TotalWordCount,
            statistics.TotalSymbolCount
        );

        reportRepository.Add(report);

        return new ReportDtoOut(
            report.FileId,
            report.FileName,
            report.ParagraphCount,
            report.TotalWordCount,
            report.TotalSymbolCount
        );
    }
}