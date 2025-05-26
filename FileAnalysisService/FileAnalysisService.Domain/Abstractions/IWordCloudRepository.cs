using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Domain.Abstractions;

public interface IWordCloudRepository
{
    WordCloud? GetById(Guid id);

    void Add(WordCloud wordCloud);
}