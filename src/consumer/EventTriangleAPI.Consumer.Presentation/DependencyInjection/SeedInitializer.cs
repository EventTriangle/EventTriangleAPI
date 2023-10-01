using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.Presentation.DependencyInjection;

public static class SeedInitializer
{
    public static void InitializeSeeds(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

        var areThereUsersSeeds = context.UserEntities.Any();

        if (areThereUsersSeeds)
        {
            return;
        }
        
        var aliceWallet = new WalletEntity(1000);
        var bobWallet = new WalletEntity(2000);
        var alexWallet = new WalletEntity(200000);
        var dimaWallet = new WalletEntity(50000000);
        var sashaWallet = new WalletEntity(44332);
        
        var users = new List<UserEntity>
        {
            new(Guid.NewGuid().ToString(), "alice@gmail.com", aliceWallet.Id, UserRole.User, UserStatus.Active),
            new(Guid.NewGuid().ToString(), "bob@gmail.com", bobWallet.Id, UserRole.User, UserStatus.Suspended),
            new(Guid.NewGuid().ToString(), "alex@gmail.com", alexWallet.Id, UserRole.Admin, UserStatus.Active),
            new(Guid.NewGuid().ToString(), "dima@gmail.com", dimaWallet.Id, UserRole.Admin, UserStatus.Active),
            new(Guid.NewGuid().ToString(), "sasha@gmail.com", sashaWallet.Id, UserRole.User, UserStatus.Active),
        };

        context.WalletEntities.AddRange(aliceWallet, bobWallet, alexWallet, dimaWallet, sashaWallet);
        context.UserEntities.AddRange(users);

        context.SaveChanges();
    }
}