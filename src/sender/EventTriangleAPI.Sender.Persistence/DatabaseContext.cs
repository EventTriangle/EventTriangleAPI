using EventTriangleAPI.Shared.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Sender.Persistence;

public class DatabaseContext : DbContext
{  
    public DbSet<ContactCreatedEvent> ContactCreatedEvents { get; set; }
    public DbSet<ContactDeletedEvent> ContactDeletedEvents { get; set; }
    public DbSet<CreditCardAddedEvent> CreditCardAddedEvents { get; set; }
    public DbSet<CreditCardChangedEvent> CreditCardChangedEvents { get; set; }
    public DbSet<CreditCardDeletedEvent> CreditCardDeletedEvents { get; set; }
    public DbSet<SupportTicketOpenedEvent> SupportTicketOpenedEvents { get; set; }
    public DbSet<SupportTicketResolvedEvent> SupportTicketResolvedEvents { get; set; }
    public DbSet<TransactionCreatedEvent> TransactionCreatedEvents { get; set; }
    public DbSet<TransactionRollBackedEvent> TransactionRollBackedEvents { get; set; }
    public DbSet<UserCreatedEvent> UserCreatedEvents { get; set; }
    public DbSet<UserSuspendedEvent> UserSuspendedEvents { get; set; }
    public DbSet<UserNotSuspendedEvent> UserNotSuspendedEvents { get; set; }
    public DbSet<UserRoleUpdatedEvent> UserRoleUpdatedEvents { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}