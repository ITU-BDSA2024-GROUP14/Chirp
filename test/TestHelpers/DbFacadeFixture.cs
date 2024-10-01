using Chirp.Core;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;

namespace TestHelpers;

public class DbFacadeFixture
{
    public IDatabase Db { get; private set; }
    public DBFacade DbFacade { get; private set; }

    public DbFacadeFixture()
    {
        Db = new TestDatabase();
        DbFacade = new DBFacade(Db);
    }

    public void Reset()
    {
        Db.Reset();
    }

    public void SeedTestData()
    {
        var embeddedProvider = new EmbeddedFileProvider(typeof(IDatabase).Assembly);
        using var reader = embeddedProvider.GetFileInfo("data/dump.sql").CreateReadStream();
        using var sr = new StreamReader(reader);

        var query = sr.ReadToEnd();

        using var connection = new SqliteConnection(Db.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        command.ExecuteNonQuery();
    }
}