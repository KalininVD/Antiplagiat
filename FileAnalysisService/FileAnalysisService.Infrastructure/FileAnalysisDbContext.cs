using Microsoft.EntityFrameworkCore;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Infrastructure;

public class FileAnalysisDbContext(DbContextOptions<FileAnalysisDbContext> options) : DbContext(options)
{
    public DbSet<AnalyticalReport> Reports { get; set; }

    public DbSet<WordCloud> WordClouds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileAnalysisDbContext).Assembly);
        modelBuilder.HasDefaultSchema("file-analysis");
        base.OnModelCreating(modelBuilder);
    }
}