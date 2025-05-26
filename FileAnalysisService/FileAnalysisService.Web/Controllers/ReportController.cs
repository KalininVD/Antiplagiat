using FileAnalysisService.Application.Services;
using FileAnalysisService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FileAnalysisService.Web.Controllers;

[ApiController]
public class ReportController(IReportService reportService, IWordCloudService wordCloudService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    private readonly IWordCloudService _wordCloudService = wordCloudService;

    [HttpGet("reports/{id}")]
    [SwaggerOperation(Summary = "Get report for a text file by FileId", Description = "Retrieves a report by FileId")]
    [SwaggerResponse(200, "Report found (or created) and returned (several statistics)")]
    [SwaggerResponse(404, "File with the specified FileId not found")]
    public IActionResult GetReport(Guid id)
    {
        var report = _reportService.GetReport(new ReportDtoIn(id));

        if (report == null)
        {
            return NotFound("File with the specified FileId not found to create a report.");
        }

        return Ok(report);
    }

    [HttpGet("reports/{id}/wordcloud")]
    [SwaggerOperation(Summary = "Get word cloud for a text file by FileId", Description = "Retrieves a word cloud by FileId")]
    [SwaggerResponse(200, "Word cloud found (or created) and returned as PNG image")]
    [SwaggerResponse(404, "File with the specified FileId not found")]
    public IActionResult GetWordCloud(Guid id)
    {
        var wordCloud = _wordCloudService.GetWordCloud(new WordCloudDtoIn(id));

        if (wordCloud == null)
        {
            return NotFound("File with the specified FileId not found to create a word cloud.");
        }

        return PhysicalFile(wordCloud.FilePath, "image/png", wordCloud.FileName);
    }
}