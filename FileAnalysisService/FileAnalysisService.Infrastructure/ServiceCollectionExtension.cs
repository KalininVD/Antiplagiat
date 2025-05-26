using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileAnalysisService.Infrastructure.Repositories;
using FileAnalysisService.Domain.Abstractions;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Domain.Services;

namespace FileAnalysisService.Infrastructure;

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

        services.AddDbContext<FileAnalysisDbContext>(
            options => options.UseNpgsql(connectionString));
        
        services.AddScoped<IReportRepository, ReportRepository>();

        var fileStoringBaseUrl = configuration["FileStorage:BaseUrl"];

        if (string.IsNullOrEmpty(fileStoringBaseUrl))
        {
            throw new InvalidOperationException("File storage base URL is not configured.");
        }

        fileStoringBaseUrl = Environment.ExpandEnvironmentVariables(fileStoringBaseUrl);

        services.AddHttpClient<IFileStoringClient, FileStoringClient>(client =>
        {
            client.BaseAddress = new Uri(fileStoringBaseUrl);
        });

        var wordCloudBaseUrl = configuration["WordCloud:BaseUrl"];

        if (string.IsNullOrEmpty(wordCloudBaseUrl))
        {
            throw new InvalidOperationException("Word cloud base URL is not configured.");
        }

        services.AddHttpClient<IWordCloudClient, WordCloudClient>(client =>
        {
            client.BaseAddress = new Uri(wordCloudBaseUrl);
        });

        var wordCloudStorageBasePath = configuration["WordCloud:StorageBasePath"];

        if (string.IsNullOrEmpty(wordCloudStorageBasePath))
        {
            throw new InvalidOperationException("Word cloud storage base path is not configured.");
        }

        if (!Directory.Exists(wordCloudStorageBasePath))
        {
            Directory.CreateDirectory(wordCloudStorageBasePath);
        }

        services.AddSingleton<IWordCloudStorageProvider>(
            provider => new WordCloudStorageProvider(wordCloudStorageBasePath));
        
        services.AddSingleton<IFileStatisticsService, FileStatisticsService>();

        services.AddScoped<IWordCloudRepository, WordCloudRepository>();

        services.AddSingleton<IReportService, ReportService>();

        services.AddSingleton<IWordCloudService, WordCloudService>();

        return services;
    }
}