namespace FileAnalysisService.Domain.Entities;

public class WordCloud(Guid fileId, string filePath, string fileName)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid FileId { get; init; } = fileId;

    public string FilePath { get; init; } = filePath;
    
    public string FileName { get; init; } = fileName;
}