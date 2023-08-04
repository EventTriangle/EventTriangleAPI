using EventTriangleAPI.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Authorization.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserSessionEntity> UserSessions { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
    }
	
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasMany(x => x.UserSessionEntities)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>().Property(x => x.Id).IsRequired();
        modelBuilder.Entity<UserEntity>().Property(x => x.Email).IsRequired();
        
        modelBuilder.Entity<UserSessionEntity>().Property(x => x.Id).IsRequired();
        modelBuilder.Entity<UserSessionEntity>().Property(x => x.ExpiresAt).IsRequired();
        modelBuilder.Entity<UserSessionEntity>().Property(x => x.UpdatedAt).IsRequired();
        modelBuilder.Entity<UserSessionEntity>().Property(x => x.Value).IsRequired();
    }
}