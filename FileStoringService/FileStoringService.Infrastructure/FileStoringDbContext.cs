using Microsoft.EntityFrameworkCore;
using FileStoringService.Domain.Entities;

namespace FileStoringService.Infrastructure;

public class FileStoringDbContext(DbContextOptions<FileStoringDbContext> options) : DbContext(options)
{
    public DbSet<StoredFile> StoredFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileStoringDbContext).Assembly);
        modelBuilder.HasDefaultSchema("file-storing");
        base.OnModelCreating(modelBuilder);
    }
}