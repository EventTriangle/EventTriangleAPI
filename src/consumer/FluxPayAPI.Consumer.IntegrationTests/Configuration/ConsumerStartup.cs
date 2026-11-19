using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Consumer.Presentation.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxPayAPI.Consumer.IntegrationTests.Configuration;

internal static class ConsumerStartup
{
    public static ServiceProvider Initialize(string databaseConnectionString, IConfiguration configuration)
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