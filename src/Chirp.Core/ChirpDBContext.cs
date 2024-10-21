using Microsoft.EntityFrameworkCore;
using Chirp.Core.DataModel;

namespace Chirp.Core;
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
}