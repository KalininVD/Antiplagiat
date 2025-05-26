using FileStoringService.Application.DTOs;

namespace FileStoringService.Application.Services;

public interface IFileService
{
    UploadFileDtoOut? UploadFile(UploadFileDtoIn dto);

    DownloadFileDtoOut? DownloadFile(DownloadFileDtoIn dto);
}