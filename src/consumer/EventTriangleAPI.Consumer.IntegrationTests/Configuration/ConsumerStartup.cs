using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Consumer.Presentation.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventTriangleAPI.Consumer.IntegrationTests.Configuration;

public class ConsumerStartup
{
    public ServiceProvider Initialize(string databaseConnectionString, IConfiguration configuration)
    {
        var serviceCollection = new ServiceCollection()
            .AddSingleton(configuration)
            .AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(databaseConnectionString);
            })
            .AddCommandHandlers()
            .AddQueryHandlers();

        return serviceCollection.BuildServiceProvider();
    }
}