using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.IntegrationTests.Configuration;
using EventTriangleAPI.Consumer.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace EventTriangleAPI.Consumer.IntegrationTests;

public class TestFixture
{
    private readonly ConsumerStartup _consumerStartup = new();
    internal readonly DatabaseContext DatabaseContextFixture;
    internal readonly AddCreditCardCommandHandler AddCreditCardCommandHandler;
    internal readonly ChangeCreditCardCommandHandler ChangeCreditCardCommandHandler;
    internal readonly CreateContactCommandHandler CreateContactCommandHandler;
    internal readonly CreateTransactionCardToUserCommandHandler CreateTransactionCardToUserCommandHandler;
    internal readonly CreateTransactionUserToUserCommandHandler CreateTransactionUserToUserCommandHandler;
    internal readonly CreateUserCommandHandler CreateUserCommandHandler;
    internal readonly DeleteContactCommandHandler DeleteContactCommandHandler;
    internal readonly DeleteCreditCardCommandHandler DeleteCreditCardCommandHandler;
    internal readonly NotSuspendUserCommandHandler NotSuspendUserCommandHandler;
    internal readonly OpenSupportTicketCommandHandler OpenSupportTicketCommandHandler;
    internal readonly ResolveSupportTicketCommandHandler ResolveSupportTicketCommandHandler;
    internal readonly RollBackTransactionCommandHandler RollBackTransactionCommandHandler;
    internal readonly SuspendUserCommandHandler SuspendUserCommandHandler;
    internal readonly UpdateUserRoleCommandHandler UpdateUserRoleCommandHandler;

    internal readonly GetContactsBySearchQueryHandler GetContactsBySearchQueryHandler;
    internal readonly GetContactsQueryHandler GetContactsQueryHandler;
    internal readonly GetCreditCardsQueryHandler GetCreditCardsQueryHandler;
    internal readonly GetProfileByIdQueryHandler GetProfileByIdQueryHandler;
    internal readonly GetProfileQueryHandler GetProfileQueryHandler;
    internal readonly GetSupportTicketsQueryHandler GetSupportTicketsQueryHandler;
    internal readonly GetTicketsQueryHandler GetTicketsQueryHandler;
    internal readonly GetTransactionsQueryHandler GetTransactionsQueryHandler;
    internal readonly GetTransactionsBySearchQueryHandler GetTransactionsBySearchQueryHandler;
    internal readonly GetTransactionsByUserIdQueryHandler GetTransactionsByUserIdQueryHandler;
    internal readonly GetUsersBySearchQueryHandler GetUsersBySearchQueryHandler;
    internal readonly GetUsersQueryHandler GetUsersQueryHandler;

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    public TestFixture()
    {
        _postgres.StartAsync().Wait();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var databaseConnectionString = _postgres.GetConnectionString();

        var jsonConfigProvider = configuration.Providers.First(x => x.GetType() == typeof(JsonConfigurationProvider));
        jsonConfigProvider.Set(AppSettingsConstants.ShouldCreateSeeds, "false");
        jsonConfigProvider.Set(AppSettingsConstants.ShouldCreateSeedsForAdmin, "false");

        var serviceProvider = _consumerStartup.Initialize(databaseConnectionString, configuration);

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

        GetContactsBySearchQueryHandler = serviceProvider.GetRequiredService<GetContactsBySearchQueryHandler>();
        GetContactsQueryHandler = serviceProvider.GetRequiredService<GetContactsQueryHandler>();
        GetCreditCardsQueryHandler = serviceProvider.GetRequiredService<GetCreditCardsQueryHandler>();
        GetProfileByIdQueryHandler = serviceProvider.GetRequiredService<GetProfileByIdQueryHandler>();
        GetProfileQueryHandler = serviceProvider.GetRequiredService<GetProfileQueryHandler>();
        GetSupportTicketsQueryHandler = serviceProvider.GetRequiredService<GetSupportTicketsQueryHandler>();
        GetTicketsQueryHandler = serviceProvider.GetRequiredService<GetTicketsQueryHandler>();
        GetTransactionsQueryHandler = serviceProvider.GetRequiredService<GetTransactionsQueryHandler>();
        GetTransactionsByUserIdQueryHandler = serviceProvider.GetRequiredService<GetTransactionsByUserIdQueryHandler>();
        GetTransactionsBySearchQueryHandler = serviceProvider.GetRequiredService<GetTransactionsBySearchQueryHandler>();
        GetUsersBySearchQueryHandler = serviceProvider.GetRequiredService<GetUsersBySearchQueryHandler>();
        GetUsersQueryHandler = serviceProvider.GetRequiredService<GetUsersQueryHandler>();
    }
}
