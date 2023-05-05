using Microsoft.EntityFrameworkCore;

namespace VkTest.Parsistence.DbContext;

public class PgDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected PgDbContext()
    {
    }
    // dotnet ef migrations add MyFirstMigrations
    // dotnet ef database update
    public PgDbContext(DbContextOptions<PgDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserGroup> UsersGroup { get; set; } = null!;
    public DbSet<UserState> UsersState { get; set; } = null!;
    
}