using FluxPayAPI.Authorization.BusinessLogic.Services;
using FluxPayAPI.Authorization.Domain.Constants;
using FluxPayAPI.Authorization.Persistence;
using FluxPayAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace FluxPayAPI.Authorization.Presentation.DependencyInjection;

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

        var logger = serviceProvider.GetService<ILoggerFactory>();

        var tickerStore = new TicketStore(
            grpcChannelAddresses,
            serviceScopeFactory,
            tickerSerializer,
            httpClient,
            azureAdConfiguration,
            memoryCache, logger);

        var refreshBackgroundService = new RefreshBackgroundService(databaseContext, tickerStore);

        serviceCollection.AddHostedService(_ => refreshBackgroundService);

        return serviceCollection;
    }
}