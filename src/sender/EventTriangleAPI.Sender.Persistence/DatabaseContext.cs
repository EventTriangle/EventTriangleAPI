using EventTriangleAPI.Sender.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventTriangleAPI.Sender.Persistence;

public class DatabaseContext : DbContext
{  
    public DbSet<ContactEventEntity> ContactEvents { get; set; }
    public DbSet<CreditCardEventEntity> CreditCardEvents { get; set; }
    public DbSet<SupportTicketEventEntity> SupportTicketEvents { get; set; }
    public DbSet<TransactionEventEntity> TransactionEventEntities { get; set; }
    public DbSet<UserEventEntity> UserEventEntities { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}