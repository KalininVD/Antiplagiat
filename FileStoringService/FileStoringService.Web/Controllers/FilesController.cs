using FileStoringService.Application.Services;
using FileStoringService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FileStoringService.Web.Controllers;

[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpPost("files")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(Summary = "Upload text file", Description = "Uploads a .txt file")]
    [SwaggerResponse(201, "File uploaded successfully")]
    [SwaggerResponse(400, "File not selected or invalid")]
    [SwaggerResponse(500, "Error during file upload")]
    public IActionResult Upload([FromForm] UploadFileFormDto dto)
    {
        var file = dto.File;
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected");
        }

        using var reader = new StreamReader(file.OpenReadStream());
        var content = reader.ReadToEnd();

        var uploadDto = new UploadFileDtoIn(file.FileName, content);
        var result = _fileService.UploadFile(uploadDto);

        if (result == null)
        {
            return StatusCode(500, "Internal server error");
        }

        return Created($"/files/{result.Id}", result);
    }

    [HttpGet("files/{id}")]
    [SwaggerOperation(Summary = "Download text file", Description = "Downloads a .txt file by its Id")] 
    [SwaggerResponse(200, "File found and returned as .txt file")]
    [SwaggerResponse(404, "File not found")]
    public IActionResult Download(Guid id)
    {
        var fileInfo = _fileService.DownloadFile(new DownloadFileDtoIn(id));

        if (fileInfo == null)
        {
            return NotFound("File not found");
        }

        return PhysicalFile(fileInfo.FilePath, "text/plain", fileInfo.FileName);
    }
}