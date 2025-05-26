using FileStoringService.Domain.Abstractions;
using FileStoringService.Domain.Entities;

namespace FileStoringService.Infrastructure.Repositories;

public class FileRepository(FileStoringDbContext dbContext) : IFileRepository
{
    private readonly FileStoringDbContext _dbContext = dbContext;

    public StoredFile? GetById(Guid id)
    {
        return _dbContext.StoredFiles.FirstOrDefault(file => file.Id == id);
    }

    public StoredFile? GetByHash(string fileHash)
    {
        return _dbContext.StoredFiles.FirstOrDefault(file => file.FileHash == fileHash);
    }

    public void Add(StoredFile storedFile)
    {
        _dbContext.StoredFiles.Add(storedFile);
        _dbContext.SaveChanges();
    }
}