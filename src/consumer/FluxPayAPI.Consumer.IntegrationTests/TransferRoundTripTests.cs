using FluentAssertions;
using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests;

public class TransferRoundTripTests
{
    [Fact]
    public async Task TransfersInBothDirectionsUpdateBalancesAndRecordBothTransactions()
    {
        await using var db = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var aliceWallet = new WalletEntity(10);
        var bobWallet = new WalletEntity(3);
        var alice = new UserEntity("alice", "alice@example.com", aliceWallet.Id, UserRole.User, UserStatus.Active);
        var bob = new UserEntity("bob", "bob@example.com", bobWallet.Id, UserRole.User, UserStatus.Active);
        db.AddRange(aliceWallet, bobWallet, alice, bob);
        await db.SaveChangesAsync();
        var handler = new CreateTransactionUserToUserCommandHandler(db);

        var forward = await handler.HandleAsync(new CreateTransactionUserToUserCommand(alice.Id, bob.Id, 4, DateTime.UtcNow));
        forward.IsSuccess.Should().BeTrue();
        aliceWallet.Balance.Should().Be(6);
        bobWallet.Balance.Should().Be(7);
        var reverse = await handler.HandleAsync(new CreateTransactionUserToUserCommand(bob.Id, alice.Id, 2, DateTime.UtcNow));
        reverse.IsSuccess.Should().BeTrue();
        aliceWallet.Balance.Should().Be(8);
        bobWallet.Balance.Should().Be(5);
        var records = await db.TransactionEntities.ToListAsync();
        records.Should().ContainSingle(t => t.FromUserId == alice.Id && t.ToUserId == bob.Id && t.Amount == 4);
        records.Should().ContainSingle(t => t.FromUserId == bob.Id && t.ToUserId == alice.Id && t.Amount == 2);
    }
}
