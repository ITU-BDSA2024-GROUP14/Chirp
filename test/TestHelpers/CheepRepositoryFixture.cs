using Chirp.Core;
using Chirp.Core.DataModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestHelpers;

public class CheepRepositoryFixture : IDisposable
{
    public ChirpDBContext CdbContext => new(Options);
    private SqliteConnection Connection { get; }
    public DbContextOptions<ChirpDBContext> Options { get; }

    public CheepRepositoryFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
        Options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(Connection).Options;
    }

    public void Dispose()
    {
    }
}