using EventTriangleAPI.Authorization.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class DatabaseMigrator
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        
        using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
        
        if (context == null)
        {
            throw new InvalidOperationException("Database context is NULL at Migrator service.");
        }
        
        context.Database.Migrate();
    }
}