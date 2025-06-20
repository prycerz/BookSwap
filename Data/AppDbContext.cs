using Microsoft.EntityFrameworkCore;
using BookSwap.Models;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<SwapRequest> SwapRequests { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    var adminPassword = BCrypt.Net.BCrypt.HashPassword("admin123");

    modelBuilder.Entity<User>().HasData(
        new User 
        { 
            Id = 1, 
            Username = "admin", 
            PasswordHash = adminPassword, 
            Role = "Admin" 
        }
    );

    modelBuilder.Entity<UserProfile>().HasData(
        new UserProfile 
        { 
            Id = 1, 
            Username = "admin" 
        }
    );
}

}
