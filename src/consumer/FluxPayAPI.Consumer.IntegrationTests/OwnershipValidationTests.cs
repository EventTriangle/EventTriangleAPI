using FluentAssertions;
using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests;

public class OwnershipValidationTests
{
    private static DatabaseContext CreateContext() => new(new DbContextOptionsBuilder<DatabaseContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    [Fact]
    public async Task DepositRejectsAnotherUsersCard()
    {
        await using var db = CreateContext();
        var ownerWallet = new WalletEntity(0);
        var requesterWallet = new WalletEntity(0);
        var owner = new UserEntity("owner", "owner@example.com", ownerWallet.Id, UserRole.User, UserStatus.Active);
        var requester = new UserEntity("requester", "requester@example.com", requesterWallet.Id, UserRole.User, UserStatus.Active);
        var card = new CreditCardEntity(owner.Id, "Test User", "4111111111111111", "123", "12/30", PaymentNetwork.Visa);
        db.AddRange(ownerWallet, requesterWallet, owner, requester, card);
        await db.SaveChangesAsync();

        var result = await new CreateTransactionCardToUserCommandHandler(db).HandleAsync(
            new CreateTransactionCardToUserCommand(card.Id, requester.Id, 1, DateTime.UtcNow));

        result.Error.Should().BeOfType<DbEntityNotFoundError>();
        requesterWallet.Balance.Should().Be(0);
        db.TransactionEntities.Should().BeEmpty();
    }

    [Fact]
    public async Task SupportTicketRejectsAnotherUsersWallet()
    {
        await using var db = CreateContext();
        var ownerWallet = new WalletEntity(0);
        var requesterWallet = new WalletEntity(0);
        var owner = new UserEntity("owner", "owner@example.com", ownerWallet.Id, UserRole.User, UserStatus.Active);
        var requester = new UserEntity("requester", "requester@example.com", requesterWallet.Id, UserRole.User, UserStatus.Active);
        var transaction = new TransactionEntity(requester.Id, requester.Id, 1, TransactionType.FromCardToUser, DateTime.UtcNow);
        db.AddRange(ownerWallet, requesterWallet, owner, requester, transaction);
        await db.SaveChangesAsync();

        var result = await new OpenSupportTicketCommandHandler(db).HandleAsync(
            new OpenSupportTicketCommand(requester.Id, ownerWallet.Id, transaction.Id, "Test", DateTime.UtcNow));

        result.Error.Should().BeOfType<DbEntityNotFoundError>();
        db.SupportTicketEntities.Should().BeEmpty();
    }

    [Fact]
    public async Task SupportTicketRejectsAnUnrelatedTransaction()
    {
        await using var db = CreateContext();
        var ownerWallet = new WalletEntity(0);
        var requesterWallet = new WalletEntity(0);
        var owner = new UserEntity("owner", "owner@example.com", ownerWallet.Id, UserRole.User, UserStatus.Active);
        var requester = new UserEntity("requester", "requester@example.com", requesterWallet.Id, UserRole.User, UserStatus.Active);
        var transaction = new TransactionEntity(owner.Id, owner.Id, 1, TransactionType.FromCardToUser, DateTime.UtcNow);
        db.AddRange(ownerWallet, requesterWallet, owner, requester, transaction);
        await db.SaveChangesAsync();

        var result = await new OpenSupportTicketCommandHandler(db).HandleAsync(
            new OpenSupportTicketCommand(requester.Id, requesterWallet.Id, transaction.Id, "Test", DateTime.UtcNow));

        result.Error.Should().BeOfType<DbEntityNotFoundError>();
        db.SupportTicketEntities.Should().BeEmpty();
    }
}
