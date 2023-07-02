using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Shared.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventTriangleAPI.Authorization.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>();

        var appSettingsService = new AppSettingsService();
        var appSettingsPath = appSettingsService.GetAppSettingsPathAuthorization();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build();

        var databaseConnectionString = configuration[AppSettingsConstants.DatabaseConnectionString];
        
        options.UseNpgsql(databaseConnectionString);

        return new DatabaseContext(options.Options);
    }
}