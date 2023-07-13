using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Sender.Persistence;

public class DatabaseContext : DbContext
{  

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}