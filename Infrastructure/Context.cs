using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context : DbContext
{
    private readonly string _connectionString;

    public Context(string connectionString)
    {
        _connectionString = connectionString;
        Database.EnsureCreated();
    }

    public DbSet<Analysis> Analyses { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}
