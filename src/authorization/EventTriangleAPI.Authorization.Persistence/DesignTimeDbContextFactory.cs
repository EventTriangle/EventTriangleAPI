using EventTriangleAPI.Authorization.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventTriangleAPI.Authorization.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var databaseConnectionString = configuration[AppSettingsConstants.DatabaseConnectionString];
        
        options.UseNpgsql(databaseConnectionString);

        return new DatabaseContext(options.Options);
    }
}