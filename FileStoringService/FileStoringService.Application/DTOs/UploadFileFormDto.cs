using Microsoft.AspNetCore.Http;

namespace FileStoringService.Application.DTOs;

public record UploadFileFormDto(IFormFile File);