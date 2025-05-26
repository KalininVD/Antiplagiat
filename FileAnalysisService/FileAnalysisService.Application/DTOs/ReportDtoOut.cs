namespace FileAnalysisService.Application.DTOs;

public record ReportDtoOut(Guid FileId, string FileName, int ParagraphCount, int TotalWordCount, int TotalSymbolCount);