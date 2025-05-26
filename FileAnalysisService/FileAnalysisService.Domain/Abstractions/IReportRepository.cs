using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Abstractions;

public interface IReportRepository
{
    AnalyticalReport? GetById(Guid id);
    
    void Add(AnalyticalReport analyticalReport);
}