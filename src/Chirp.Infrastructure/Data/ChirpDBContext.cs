using Chirp.Core.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Data;

/// <summary>
/// Represents the database context for the Chirp application.
/// </summary>
public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();

        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(Cheep.MaxLength);
    }
}