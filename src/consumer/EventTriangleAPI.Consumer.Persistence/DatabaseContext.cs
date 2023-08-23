using EventTriangleAPI.Consumer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<ContactEntity> ContactEntities { get; set; }
    public DbSet<CreditCardEntity> CreditCardEntities { get; set; }
    public DbSet<SupportTicketEntity> SupportTicketEntities { get; set; }
    public DbSet<TransactionEntity> TransactionEntities { get; set; }
    public DbSet<WalletEntity> WalletEntities { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ContactEntity>()
            .HasKey(x => new {x.UserId, x.ContactId});
        
        modelBuilder
            .Entity<ContactEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<ContactEntity>()
            .HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<CreditCardEntity>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<SupportTicketEntity>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<SupportTicketEntity>()
            .HasOne(x => x.Wallet)
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<SupportTicketEntity>()
            .HasOne(x => x.Transaction)
            .WithOne()
            .HasForeignKey<SupportTicketEntity>(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<TransactionEntity>()
            .HasOne(x => x.FromUser)
            .WithMany()
            .HasForeignKey(x => x.FromUserId);
        
        modelBuilder
            .Entity<TransactionEntity>()
            .HasOne(x => x.ToUser)
            .WithMany()
            .HasForeignKey(x => x.ToUserId);

        modelBuilder
            .Entity<UserEntity>()
            .HasOne(x => x.Wallet)
            .WithOne(x => x.User)
            .HasForeignKey<UserEntity>(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}