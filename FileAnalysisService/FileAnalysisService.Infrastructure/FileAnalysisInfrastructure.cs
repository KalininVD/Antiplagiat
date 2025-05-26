using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysisService.Infrastructure;

public static class FileAnalysisInfrastructure
{
    public static void Migrate(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<FileAnalysisDbContext>().Database.Migrate();
    }
}