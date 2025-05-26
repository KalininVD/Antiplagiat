using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Infrastructure.Repositories;

public class WordCloudRepository(FileAnalysisDbContext dbContext) : IWordCloudRepository
{
    private readonly FileAnalysisDbContext _dbContext = dbContext;

    public WordCloud? GetById(Guid fileId)
    {
        return _dbContext.WordClouds.FirstOrDefault(w => w.FileId == fileId);
    }

    public void Add(WordCloud wordCloud)
    {
        _dbContext.WordClouds.Add(wordCloud);
        _dbContext.SaveChanges();
    }
}