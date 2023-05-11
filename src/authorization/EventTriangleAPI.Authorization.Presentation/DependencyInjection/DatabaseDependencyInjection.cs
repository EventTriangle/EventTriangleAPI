using EventTriangleAPI.Authorization.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class DatabaseDependencyInjection
{
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection serviceCollection,
        string connectionString)
    {
        serviceCollection.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return serviceCollection;
    }
}