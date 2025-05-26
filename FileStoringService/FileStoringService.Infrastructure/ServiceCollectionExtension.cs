using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileStoringService.Application.Services;
using FileStoringService.Domain.Abstractions;
using FileStoringService.Domain.Services;
using FileStoringService.Infrastructure.Repositories;

namespace FileStoringService.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'PostgreSQL' is missing.");
        }

        connectionString = Environment.ExpandEnvironmentVariables(connectionString);

        services.AddDbContext<FileStoringDbContext>(
            options => options.UseNpgsql(connectionString));

        services.AddScoped<IFileRepository, FileRepository>();

        var fileStorageBasePath = configuration["FileStorage:BasePath"];

        if (string.IsNullOrEmpty(fileStorageBasePath))
        {
            throw new InvalidOperationException("File storage base path is not configured.");
        }

        if (!Directory.Exists(fileStorageBasePath))
        {
            Directory.CreateDirectory(fileStorageBasePath);
        }

        services.AddSingleton<IFileStorageProvider>(
            provider => new FileStorageProvider(fileStorageBasePath));

        services.AddSingleton<IFileHashingService, FileHashingService>();

        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}