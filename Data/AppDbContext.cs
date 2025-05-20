using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var adminPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = adminPassword, Role = "Admin" }
        );
    }
}
