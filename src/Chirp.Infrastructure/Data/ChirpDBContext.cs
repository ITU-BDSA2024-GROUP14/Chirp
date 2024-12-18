using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Data;

/// <summary>
///     Represents the database context for the Chirp application.
/// </summary>
public class ChirpDBContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }

    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    ///     This method is called by EFCore to configure the database.
    ///     This is where we define the relationships between the entities and the constraints on the database.
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder from EFCore used to configure the database</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasIndex(a => a.DisplayName).IsUnique();
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Author>().Ignore(a => a.AuthorId);
        modelBuilder.Entity<OriginalCheep>().Property("_text").HasMaxLength(Cheep.MaxLength);

        modelBuilder.Entity<Author>().HasMany(a => a.Following).WithMany().UsingEntity(j => j.ToTable("Follows"));

        modelBuilder.Entity<Cheep>()
            .HasDiscriminator<string>("Type")
            .HasValue<OriginalCheep>(nameof(OriginalCheep))
            .HasValue<RepostCheep>(nameof(RepostCheep));

        base.OnModelCreating(modelBuilder);
    }
}