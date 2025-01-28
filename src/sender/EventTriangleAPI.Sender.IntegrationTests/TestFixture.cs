using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.IntegrationTests.Configuration;
using EventTriangleAPI.Sender.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests;

[Collection("Sequential")]
public class TestFixture
{
    private readonly SenderStartup _senderStartup = new();
    internal readonly DatabaseContext DatabaseContextFixture;

    internal readonly CreateUserCommandHandler CreateUserCommandHandler;
    internal readonly AddContactCommandHandler AddContactCommandHandler;
    internal readonly AttachCreditCardToAccountCommandHandler AttachCreditCardToAccountCommandHandler;
    internal readonly DeleteContactCommandHandler DeleteContactCommandHandler;
    internal readonly DeleteCreditCardCommandHandler DeleteCreditCardCommandHandler;
    internal readonly EditCreditCardCommandHandler EditCreditCardCommandHandler;
    internal readonly NotSuspendUserCommandHandler NotSuspendUserCommandHandler;
    internal readonly OpenSupportTicketCommandHandler OpenSupportTicketCommandHandler;
    internal readonly ResolveSupportTicketCommandHandler ResolveSupportTicketCommandHandler;
    internal readonly RollBackTransactionCommandHandler RollBackTransactionCommandHandler;
    internal readonly SuspendUserCommandHandler SuspendUserCommandHandler;
    internal readonly TopUpAccountBalanceCommandHandler TopUpAccountBalanceCommandHandler;
    internal readonly UpdateUserRoleCommandHandler UpdateUserRoleCommandHandler;
    internal readonly CreateTransactionUserToUserCommandHandler CreateTransactionUserToUserCommandHandler;

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    public TestFixture()
    {
        _postgres.StartAsync().Wait();

        var databaseConnectionString = _postgres.GetConnectionString();

        var serviceProvider = _senderStartup.Initialize(databaseConnectionString);

        DatabaseContextFixture = serviceProvider.GetRequiredService<DatabaseContext>() ??
                                 throw new InvalidOperationException("DatabaseContext service is not registered in the DI.");

        CreateUserCommandHandler = serviceProvider.GetRequiredService<CreateUserCommandHandler>();
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
        CreateTransactionUserToUserCommandHandler = serviceProvider.GetRequiredService<CreateTransactionUserToUserCommandHandler>();
    }
}
