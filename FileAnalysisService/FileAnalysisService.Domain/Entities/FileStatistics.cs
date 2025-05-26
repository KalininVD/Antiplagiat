namespace FileAnalysisService.Domain.Entities;

public class FileStatistics(int ParagraphCount, int TotalWordCount, int TotalSymbolCount)
{
    public int ParagraphCount { get; init; } = ParagraphCount;

    public int TotalWordCount { get; init; } = TotalWordCount;
    
    public int TotalSymbolCount { get; init; } = TotalSymbolCount;
}