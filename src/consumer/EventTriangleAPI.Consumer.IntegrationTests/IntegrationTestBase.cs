using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.IntegrationTests.Configuration;
using EventTriangleAPI.Consumer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests;

public class IntegrationTestBase : IAsyncLifetime
{
    private readonly ConsumerStartup _consumerStartup = new();
    protected readonly DatabaseContext DatabaseContextFixture;
    protected readonly AddCreditCardCommandHandler AddCreditCardCommandHandler;
    protected readonly ChangeCreditCardCommandHandler ChangeCreditCardCommandHandler;
    protected readonly CreateContactCommandHandler CreateContactCommandHandler;
    protected readonly CreateTransactionCardToUserCommandHandler CreateTransactionCardToUserCommandHandler;
    protected readonly CreateTransactionUserToUserCommandHandler CreateTransactionUserToUserCommandHandler;
    protected readonly CreateUserCommandHandler CreateUserCommandHandler;
    protected readonly DeleteContactCommandHandler DeleteContactCommandHandler;
    protected readonly DeleteCreditCardCommandHandler DeleteCreditCardCommandHandler;
    protected readonly NotSuspendUserCommandHandler NotSuspendUserCommandHandler;
    protected readonly OpenSupportTicketCommandHandler OpenSupportTicketCommandHandler;
    protected readonly ResolveSupportTicketCommandHandler ResolveSupportTicketCommandHandler;
    protected readonly RollBackTransactionCommandHandler RollBackTransactionCommandHandler;
    protected readonly SuspendUserCommandHandler SuspendUserCommandHandler;
    protected readonly UpdateUserRoleCommandHandler UpdateUserRoleCommandHandler;
    
    public IntegrationTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var databaseConnectionString = configuration[AppSettingsConstants.DatabaseConnectionStringIntegrationTests];

        var serviceProvider = _consumerStartup.Initialize(databaseConnectionString);

        DatabaseContextFixture = serviceProvider.GetRequiredService<DatabaseContext>();

        AddCreditCardCommandHandler = serviceProvider.GetRequiredService<AddCreditCardCommandHandler>();
        ChangeCreditCardCommandHandler = serviceProvider.GetRequiredService<ChangeCreditCardCommandHandler>();
        CreateContactCommandHandler = serviceProvider.GetRequiredService<CreateContactCommandHandler>();
        CreateTransactionCardToUserCommandHandler = serviceProvider.GetRequiredService<CreateTransactionCardToUserCommandHandler>();
        CreateTransactionUserToUserCommandHandler = serviceProvider.GetRequiredService<CreateTransactionUserToUserCommandHandler>();
        CreateUserCommandHandler = serviceProvider.GetRequiredService<CreateUserCommandHandler>();
        DeleteContactCommandHandler = serviceProvider.GetRequiredService<DeleteContactCommandHandler>();
        DeleteCreditCardCommandHandler = serviceProvider.GetRequiredService<DeleteCreditCardCommandHandler>();
        NotSuspendUserCommandHandler = serviceProvider.GetRequiredService<NotSuspendUserCommandHandler>();
        OpenSupportTicketCommandHandler = serviceProvider.GetRequiredService<OpenSupportTicketCommandHandler>();
        ResolveSupportTicketCommandHandler = serviceProvider.GetRequiredService<ResolveSupportTicketCommandHandler>();
        RollBackTransactionCommandHandler = serviceProvider.GetRequiredService<RollBackTransactionCommandHandler>();
        SuspendUserCommandHandler = serviceProvider.GetRequiredService<SuspendUserCommandHandler>();
        UpdateUserRoleCommandHandler = serviceProvider.GetRequiredService<UpdateUserRoleCommandHandler>();
    }

    public async Task InitializeAsync()
    {
        await DatabaseContextFixture.Database.MigrateAsync();
                
        const string sql = "TRUNCATE TABLE \"ContactEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"UserEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"WalletEntities\" CASCADE;";
        
        await DatabaseContextFixture.Database.ExecuteSqlRawAsync(sql);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}