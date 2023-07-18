using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.Domain.Constants;
using EventTriangleAPI.Sender.IntegrationTests.Configuration;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests;

[Collection("Sequential")]
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly AppSettingsService _appSettingsService = new();
    private readonly SenderStartup _senderStartup = new();
    protected readonly DatabaseContext DatabaseContextFixture;

    protected readonly AddContactCommandHandler AddContactCommandHandler;
    protected readonly AttachCreditCardToAccountCommandHandler AttachCreditCardToAccountCommandHandler;
    protected readonly DeleteContactCommandHandler DeleteContactCommandHandler;
    protected readonly DeleteCreditCardCommandHandler DeleteCreditCardCommandHandler;
    protected readonly EditCreditCardCommandHandler EditCreditCardCommandHandler;
    protected readonly NotSuspendUserCommandHandler NotSuspendUserCommandHandler;
    protected readonly OpenSupportTicketCommandHandler OpenSupportTicketCommandHandler;
    protected readonly ResolveSupportTicketCommandHandler ResolveSupportTicketCommandHandler;
    protected readonly RollBackTransactionCommandHandler RollBackTransactionCommandHandler;
    protected readonly SuspendUserCommandHandler SuspendUserCommandHandler;
    protected readonly TopUpAccountBalanceCommandHandler TopUpAccountBalanceCommandHandler;
    protected readonly UpdateUserRoleCommandHandler UpdateUserRoleCommandHandler;
    
    protected IntegrationTestBase()
    {
        var appSettingsPath = _appSettingsService.GetAppSettingsPathSender();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build();

        var databaseConnectionString = configuration[AppSettingsConstants.DatabaseConnectionStringIntegrationTests];

        var serviceProvider = _senderStartup.Initialize(databaseConnectionString);
        
        DatabaseContextFixture = serviceProvider.GetRequiredService<DatabaseContext>() ??
                                 throw new InvalidOperationException("DatabaseContext service is not registered in the DI.");

        AddContactCommandHandler = serviceProvider.GetRequiredService<AddContactCommandHandler>();
        AttachCreditCardToAccountCommandHandler = serviceProvider.GetRequiredService<AttachCreditCardToAccountCommandHandler>();
        DeleteContactCommandHandler = serviceProvider.GetRequiredService<DeleteContactCommandHandler>();
        DeleteCreditCardCommandHandler = serviceProvider.GetRequiredService<DeleteCreditCardCommandHandler>();
        EditCreditCardCommandHandler = serviceProvider.GetRequiredService<EditCreditCardCommandHandler>();
        NotSuspendUserCommandHandler = serviceProvider.GetRequiredService<NotSuspendUserCommandHandler>();
        OpenSupportTicketCommandHandler = serviceProvider.GetRequiredService<OpenSupportTicketCommandHandler>();
        ResolveSupportTicketCommandHandler = serviceProvider.GetRequiredService<ResolveSupportTicketCommandHandler>();
        RollBackTransactionCommandHandler = serviceProvider.GetRequiredService<RollBackTransactionCommandHandler>();
        SuspendUserCommandHandler = serviceProvider.GetRequiredService<SuspendUserCommandHandler>();
        TopUpAccountBalanceCommandHandler = serviceProvider.GetRequiredService<TopUpAccountBalanceCommandHandler>();
        UpdateUserRoleCommandHandler = serviceProvider.GetRequiredService<UpdateUserRoleCommandHandler>();
    }
    
    public async Task InitializeAsync()
    {
        await DatabaseContextFixture.Database.MigrateAsync();
        
        const string sql = "TRUNCATE TABLE \"ContactCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"ContactDeletedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardAddedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardChangedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardDeletedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketOpenedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketResolvedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionRollBackedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserSuspendedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserNotSuspendedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserRoleUpdatedEvents\" CASCADE;";

        await DatabaseContextFixture.Database.ExecuteSqlRawAsync(sql);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}