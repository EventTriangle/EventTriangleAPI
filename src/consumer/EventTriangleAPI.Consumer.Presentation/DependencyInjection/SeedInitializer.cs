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
        var transactionList = new List<TransactionEntity>();

        var random = new Random();
        
        for (var i = 0; i < 45; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(dima.Id, bob.Id, 10000, TransactionType.FromUserToUser, DateTime.UtcNow) 
                : new TransactionEntity(bob.Id, dima.Id, 10000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (int i = 0; i < 20; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(alex.Id, alice.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow)
                : new TransactionEntity(alice.Id, alex.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (int i = 0; i < 20; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(dima.Id, sasha.Id, 700, TransactionType.FromUserToUser, DateTime.UtcNow)
                : new TransactionEntity(sasha.Id, dima.Id, 700, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (int i = 0; i < 10; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(dima.Id, bob.Id, 8000, TransactionType.FromUserToUser, DateTime.UtcNow)
                : new TransactionEntity(bob.Id, dima.Id, 8000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (int i = 0; i < 15; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(bob.Id, sasha.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow)
                : new TransactionEntity(sasha.Id, bob.Id, 1000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (int i = 0; i < 15; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(dima.Id, alex.Id, 7775, TransactionType.FromUserToUser, DateTime.UtcNow)
                : new TransactionEntity(alex.Id, dima.Id, 7775, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }

        context.TransactionEntities.AddRange(transactionList);
        
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