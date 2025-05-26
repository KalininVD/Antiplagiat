using FileStoringService.Domain.Entities;

namespace FileStoringService.Domain.Abstractions;

public interface IFileRepository
{
    StoredFile? GetById(Guid id);

    StoredFile? GetByHash(string fileHash);

    void Add(StoredFile storedFile);
}