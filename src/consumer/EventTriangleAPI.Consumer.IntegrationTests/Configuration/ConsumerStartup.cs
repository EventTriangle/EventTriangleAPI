using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Consumer.Presentation.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventTriangleAPI.Consumer.IntegrationTests.Configuration;

public class ConsumerStartup
{
    public ServiceProvider Initialize(string databaseConnectionString)
    {
        var serviceCollection = new ServiceCollection()
            .AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(databaseConnectionString);
            })
            .AddCommandHandlers();

        return serviceCollection.BuildServiceProvider();
    }
}