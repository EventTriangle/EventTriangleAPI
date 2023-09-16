using EventTriangleAPI.Authorization.BusinessLogic.Services;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Authorization.Persistence;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class HostedServicesDependencyInjection
{
    public static IServiceCollection AddHostedServices(this IServiceCollection serviceCollection)
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var configuration = serviceProvider.GetService<IConfiguration>();
        var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
        var databaseContext = serviceProvider.GetService<DatabaseContext>();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();

        var tickerSerializer = new TicketSerializer();
        var httpClient = new HttpClient();

        var azAdSection = configuration.GetSection(AppSettingsConstants.AzureAdSelection);
        var azureAdConfiguration = azAdSection.Get<AzureAdConfiguration>();
        var adClientSecret = Environment.GetEnvironmentVariable(AppSettingsConstants.AdSecretKey);
        azureAdConfiguration.ClientSecret = adClientSecret;

        var grpcChannelAddresses = configuration[AppSettingsConstants.GrpcChannelAddresses];

        var tickerStore = new TicketStore(
            grpcChannelAddresses,
            serviceScopeFactory,
            tickerSerializer,
            httpClient,
            azureAdConfiguration,
            memoryCache);

        var refreshBackgroundService = new RefreshBackgroundService(databaseContext, tickerStore);

        serviceCollection.AddHostedService(_ => refreshBackgroundService);

        return serviceCollection;
    }
}