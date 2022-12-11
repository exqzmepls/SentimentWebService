using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Analysis> Analyses { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;
}
