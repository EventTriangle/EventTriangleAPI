using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

namespace EventTriangleAPI.Consumer.Presentation.DependencyInjection;

public static class CommandHandlersDependencyInjection
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AddCreditCardCommandHandler>();
        serviceCollection.AddScoped<ChangeCreditCardCommandHandler>();
        serviceCollection.AddScoped<CreateContactCommandHandler>();
        serviceCollection.AddScoped<CreateTransactionCardToUserCommandHandler>();
        serviceCollection.AddScoped<CreateTransactionUserToUserCommandHandler>();
        serviceCollection.AddScoped<CreateUserCommandHandler>();
        serviceCollection.AddScoped<DeleteContactCommandHandler>();
        serviceCollection.AddScoped<DeleteCreditCardCommandHandler>();
        serviceCollection.AddScoped<NotSuspendUserCommandHandler>();
        serviceCollection.AddScoped<OpenSupportTicketCommandHandler>();
        serviceCollection.AddScoped<ResolveSupportTicketCommandHandler>();
        serviceCollection.AddScoped<RollBackTransactionCommandHandler>();
        serviceCollection.AddScoped<SuspendUserCommandHandler>();
        serviceCollection.AddScoped<UpdateUserRoleCommandHandler>();
        
        return serviceCollection;
    }
}