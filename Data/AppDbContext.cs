using Microsoft.EntityFrameworkCore;
using ChocoJoy.Models;

namespace ChocoJoy.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
}
