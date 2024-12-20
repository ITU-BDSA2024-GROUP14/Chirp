using Chirp.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestHelpers;

public class ChirpDbContextFixture : IDisposable
{
    public ChirpDbContextFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
        Options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(Connection).Options;
    }

    private SqliteConnection Connection { get; }
    private DbContextOptions<ChirpDBContext> Options { get; }

    public void Dispose()
    {
        Connection.Dispose();
    }

    public ChirpDBContext CreateContext()
    {
        var context = new ChirpDBContext(Options);
        return context;
    }

    public void Reset()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void SeedDatabase()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();
        var dbInitializer = new TestDbInitializer(context);
        dbInitializer.Seed();
    }
}