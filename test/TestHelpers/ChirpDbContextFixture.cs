using Chirp.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestHelpers;

public class ChirpDbContextFixture : IDisposable
{
    public ChirpDBContext CreateContext()
    {
        var context = new ChirpDBContext(Options);
        return context;
    }

    private SqliteConnection Connection { get; }
    private DbContextOptions<ChirpDBContext> Options { get; }

    public ChirpDbContextFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
        Options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(Connection).Options;
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

        var TestData = new TestData();
        TestData.SimpleSeedDatabase(context);
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}