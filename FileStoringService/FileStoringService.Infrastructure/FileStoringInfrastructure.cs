using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.Infrastructure;

public static class FileStoringInfrastructure
{
    public static void Migrate(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<FileStoringDbContext>().Database.Migrate();
    }
}