using Microsoft.EntityFrameworkCore;
using Chirp.Core.DataModel;

namespace Chirp.Core;

public class ChirpDBContext : DbContext
{
    internal DbSet<Cheep> Cheeps { get; set; }
    internal DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }
}