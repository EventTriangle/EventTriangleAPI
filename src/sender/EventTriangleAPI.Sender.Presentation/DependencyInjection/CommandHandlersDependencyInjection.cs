using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

namespace EventTriangleAPI.Sender.Presentation.DependencyInjection;

public static class CommandHandlersDependencyInjection
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddContactCommandHandler>();
        serviceCollection.AddScoped<AttachCreditCardToAccountCommandHandler>();
        serviceCollection.AddScoped<DeleteContactCommandHandler>();
        serviceCollection.AddScoped<DeleteCreditCardCommandHandler>();
        serviceCollection.AddScoped<EditCreditCardCommandHandler>();
        serviceCollection.AddScoped<NotSuspendUserCommandHandler>();
        serviceCollection.AddScoped<OpenSupportTicketCommandHandler>();
        serviceCollection.AddScoped<ResolveSupportTicketCommandHandler>();
        serviceCollection.AddScoped<RollBackTransactionCommandHandler>();
        serviceCollection.AddScoped<SuspendUserCommandHandler>();
        serviceCollection.AddScoped<TopUpAccountBalanceCommandHandler>();
        serviceCollection.AddScoped<UpdateUserRoleCommandHandler>();
        
        return serviceCollection;
    }
}