using EventTriangleAPI.Authorization.BusinessLogic.Services;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class TicketStoreDependencyInjection
{
    public static IServiceCollection AddTicketStore(this IServiceCollection serviceCollection)
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var configuration = serviceProvider.GetService<IConfiguration>();
        var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
        var ticketSerializer = new TicketSerializer();
        var httpClient = new HttpClient();
        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        
        var azAdSection = configuration.GetSection(AppSettingsConstants.AzureAdSelection);
        var azureAdConfiguration = azAdSection.Get<AzureAdConfiguration>();
        var adClientSecret = Environment.GetEnvironmentVariable(AppSettingsConstants.AdSecretKey);
        azureAdConfiguration.ClientSecret = adClientSecret;

        var grpcChannelAddresses = configuration
            .GetSection(AppSettingsConstants.GrpcChannelAddresses)
            .Get<GrpcChannelAddresses>();

        var ticketStore = new TicketStore(
            grpcChannelAddresses.SenderAddress,
            serviceScopeFactory, 
            ticketSerializer,
            httpClient,
            azureAdConfiguration,
            memoryCache);
        
        serviceCollection.AddSingleton<ITicketStore, TicketStore>(_ => ticketStore);
        serviceCollection
            .AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
            .Configure<ITicketStore>((o, _) => o.SessionStore = ticketStore);

        return serviceCollection;
    }
}