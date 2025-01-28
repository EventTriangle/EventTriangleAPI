using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Sender.Presentation.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventTriangleAPI.Sender.IntegrationTests.Configuration;

public class SenderStartup
{
    public ServiceProvider Initialize(string connectionString)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddMassTransit(config =>
        {
            config.UsingInMemory();
        });
        serviceCollection.AddCommandHandlers();
        serviceCollection.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        return serviceCollection.BuildServiceProvider();
    }
}