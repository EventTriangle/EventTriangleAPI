using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.Presentation.DependencyInjection;

public static class SeedInitializer
{
    public static async Task InitializeSeeds(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

        var areThereUsersSeeds = context.UserEntities.Any();

        if (areThereUsersSeeds)
        {
            return;
        }
        
        // user and wallet seeds 
        var aliceWallet = new WalletEntity(1000);
        var bobWallet = new WalletEntity(2000);
        var alexWallet = new WalletEntity(200000);
        var dimaWallet = new WalletEntity(50000000);
        var sashaWallet = new WalletEntity(44332);

        var alice = new UserEntity(UserSeedIdConstants.AliceId, "alice@gmail.com", aliceWallet.Id, UserRole.User, UserStatus.Active);
        var bob = new UserEntity(UserSeedIdConstants.BobId, "bob@gmail.com", bobWallet.Id, UserRole.User, UserStatus.Suspended);
        var alex = new UserEntity(UserSeedIdConstants.AlexId, "alex@gmail.com", alexWallet.Id, UserRole.User, UserStatus.Active);
        var dima = new UserEntity(UserSeedIdConstants.DimaId, "dima@gmail.com", dimaWallet.Id, UserRole.Admin, UserStatus.Active);
        var sasha = new UserEntity(UserSeedIdConstants.SashaId, "sasha@gmail.com", sashaWallet.Id, UserRole.User, UserStatus.Active);
        
        var users = new List<UserEntity> { alice, bob, alex, dima, sasha };

        context.WalletEntities.AddRange(aliceWallet, bobWallet, alexWallet, dimaWallet, sashaWallet);
        context.UserEntities.AddRange(users);

        await context.SaveChangesAsync();
        
        // transaction seeds
        var transactionsDimaToAlice = new List<TransactionEntity>();
        var transactionsAliceToDima = new List<TransactionEntity>();
        var transactionsAlexToAlice = new List<TransactionEntity>();
        var transactionsSashaToDima = new List<TransactionEntity>();
        var transactionsDimaToBob = new List<TransactionEntity>();
        var transactionsBobToSasha = new List<TransactionEntity>();
        var transactionsDimaToAlex = new List<TransactionEntity>();
        
        for (int i = 0; i < 30; i++)
        {
            var transaction = new TransactionEntity(dima.Id, alice.Id, 5000, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsDimaToAlice.Add(transaction);
        }
        
        for (int i = 0; i < 15; i++)
        {
            var transaction = new TransactionEntity(alice.Id, dima.Id, 100, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsAliceToDima.Add(transaction);
        }
        
        for (int i = 0; i < 10; i++)
        {
            var transaction = new TransactionEntity(alex.Id, alice.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsAlexToAlice.Add(transaction);
        }
        
        for (int i = 0; i < 10; i++)
        {
            var transaction = new TransactionEntity(sasha.Id, dima.Id, 700, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsSashaToDima.Add(transaction);
        }
        
        for (int i = 0; i < 10; i++)
        {
            var transaction = new TransactionEntity(dima.Id, bob.Id, 8000, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsDimaToBob.Add(transaction);
        }
        
        for (int i = 0; i < 15; i++)
        {
            var transaction = new TransactionEntity(bob.Id, sasha.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsBobToSasha.Add(transaction);
        }
        
        for (int i = 0; i < 15; i++)
        {
            var transaction = new TransactionEntity(dima.Id, alex.Id, 7775, TransactionType.FromUserToUser, DateTime.UtcNow);
            transactionsDimaToAlex.Add(transaction);
        }

        context.TransactionEntities.AddRange(transactionsDimaToAlice);
        context.TransactionEntities.AddRange(transactionsAliceToDima);
        context.TransactionEntities.AddRange(transactionsAlexToAlice);
        context.TransactionEntities.AddRange(transactionsSashaToDima);
        context.TransactionEntities.AddRange(transactionsDimaToBob);
        context.TransactionEntities.AddRange(transactionsBobToSasha);
        context.TransactionEntities.AddRange(transactionsDimaToAlex);
        
        await context.SaveChangesAsync();
        
        // make suspended users
        sasha.UpdateUserStatus(UserStatus.Suspended);
        alex.UpdateUserStatus(UserStatus.Suspended);
        
        await context.SaveChangesAsync();
        
        // make admin
        dima.UpdateUserRole(UserRole.Admin);
        
        await context.SaveChangesAsync();
    }
}