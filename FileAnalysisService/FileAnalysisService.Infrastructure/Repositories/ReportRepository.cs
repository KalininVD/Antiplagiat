using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Infrastructure.Repositories;

public class ReportRepository(FileAnalysisDbContext dbContext) : IReportRepository
{
    private readonly FileAnalysisDbContext _dbContext = dbContext;

    public AnalyticalReport? GetById(Guid id)
    {
        return _dbContext.Reports.FirstOrDefault(report => report.FileId == id);
    }

    public void Add(AnalyticalReport analyticalReport)
    {
        _dbContext.Reports.Add(analyticalReport);
        _dbContext.SaveChanges();
    }
}