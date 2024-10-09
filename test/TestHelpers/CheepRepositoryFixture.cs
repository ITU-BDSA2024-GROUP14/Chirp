using Chirp.Core;
using Chirp.Core.DataModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestHelpers;

public class CheepRepositoryFixture : IDisposable
{
    public ChirpDBContext CreateContext() => new(Options);
    private SqliteConnection Connection { get; }
    private DbContextOptions<ChirpDBContext> Options { get; }

    public CheepRepositoryFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
        Options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(Connection).Options;
    }

    public void SeedDatabase()
    {
        using (var context = new ChirpDBContext(Options))
        {
            DbInitializer.SeedDatabase(context);
        }
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}