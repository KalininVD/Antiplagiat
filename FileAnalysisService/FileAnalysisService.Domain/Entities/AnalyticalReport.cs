namespace FileAnalysisService.Domain.Entities;

public class AnalyticalReport(Guid fileId, string fileName, int paragraphCount, int totalWordCount, int totalSymbolCount)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid FileId { get; init; } = fileId;

    public string FileName { get; init; } = fileName;

    public int ParagraphCount { get; init; } = paragraphCount;

    public int TotalWordCount { get; init; } = totalWordCount;

    public int TotalSymbolCount { get; init; } = totalSymbolCount;
}