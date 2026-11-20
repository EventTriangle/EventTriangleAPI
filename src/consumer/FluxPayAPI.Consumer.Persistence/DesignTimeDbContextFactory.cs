using FluxPayAPI.Consumer.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FluxPayAPI.Consumer.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var databaseConnectionString = configuration[AppSettingsConstants.DatabaseConnectionString]
            ?? "Host=localhost;Database=ConsumerDb;Username=postgres;Password=postgres";
        
        options.UseNpgsql(databaseConnectionString);

        return new DatabaseContext(options.Options);
    }
}