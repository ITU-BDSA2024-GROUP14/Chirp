using Chirp.Core.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Data;

/// <summary>
/// Represents the database context for the Chirp application.
/// </summary>
public class ChirpDBContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }
    /// <summary>
    /// This method is called when the model for a derived context has been initialized,
    /// but before the model has been locked down and used to initialize the context.
    /// It is used to configure how the data is stored in the database.
    /// </summary>
    /// <param name="modelBuilder">This is the ModelBuilder from EFCore used to configure the database</param>
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