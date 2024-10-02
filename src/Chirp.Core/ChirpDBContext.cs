using Microsoft.EntityFrameworkCore;
using Chirp.Core.DataModel;

namespace Chirp.Core;

public class ChirpDBContext : DbContext
{
    private DbSet<Cheep> Cheeps { get; set; }
    private DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }
}