using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

namespace EventTriangleAPI.Consumer.Presentation.DependencyInjection;

public static class QueryHandlersDependencyInjection
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetContactsBySearchQueryHandler>();
        serviceCollection.AddScoped<GetContactsQueryHandler>();
        serviceCollection.AddScoped<GetCreditCardsQueryHandler>();
        serviceCollection.AddScoped<GetProfileQueryHandler>();
        serviceCollection.AddScoped<GetSupportTicketsQueryHandler>();
        serviceCollection.AddScoped<GetTicketsQueryHandler>();
        serviceCollection.AddScoped<GetTransactionsQueryHandler>();
        serviceCollection.AddScoped<GetUsersBySearchQueryHandler>();
        serviceCollection.AddScoped<GetUsersQueryHandler>();
        
        return serviceCollection;
    }
}