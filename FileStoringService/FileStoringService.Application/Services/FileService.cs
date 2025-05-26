using FileStoringService.Domain.Abstractions;
using FileStoringService.Application.DTOs;
using FileStoringService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.Application.Services;

public class FileService(IServiceScopeFactory serviceScopeFactory) : IFileService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public UploadFileDtoOut? UploadFile(UploadFileDtoIn dto)
    {
        try
        {
            string fileName = dto.FileName ?? string.Empty;
            string content = dto.Content ?? string.Empty;

            using var scope = _serviceScopeFactory.CreateScope();
            var fileRepository = scope.ServiceProvider.GetRequiredService<IFileRepository>();
            var fileStorageProvider = scope.ServiceProvider.GetRequiredService<IFileStorageProvider>();
            var fileHashingService = scope.ServiceProvider.GetRequiredService<IFileHashingService>();

            string fileHash = fileHashingService.CalculateHash(content);

            var existingFile = fileRepository.GetByHash(fileHash);
            if (existingFile != null)
            {
                return new UploadFileDtoOut(existingFile.Id, true);
            }

            var filePath = fileStorageProvider.SaveFile(fileName, content);

            var storedFile = new StoredFile(fileName, fileHash, filePath);
            fileRepository.Add(storedFile);

            return new UploadFileDtoOut(storedFile.Id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public DownloadFileDtoOut? DownloadFile(DownloadFileDtoIn dto)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var fileRepository = scope.ServiceProvider.GetRequiredService<IFileRepository>();

        var storedFile = fileRepository.GetById(dto.Id);
        if (storedFile == null)
        {
            return null;
        }

        return new DownloadFileDtoOut(storedFile.FileName, storedFile.FilePath);
    }
}