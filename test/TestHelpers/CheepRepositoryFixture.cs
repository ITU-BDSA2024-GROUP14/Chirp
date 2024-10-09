using Chirp.Core;
using Chirp.Core.DataModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestHelpers;

public class CheepRepositoryFixture : IDisposable
{
    public ChirpDBContext CdbContext { get; private set; }
    private SqliteConnection Connection { get; }
    public DbContextOptions<ChirpDBContext> Options { get; }

    public CheepRepositoryFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
        Options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(Connection).Options;

        using (var context = new ChirpDBContext(Options))
        {
            context.Database.EnsureCreated();
            context.Cheeps.AddRange(
                new Cheep
                {
                    Author = new Author { Name = "jones", AuthorId = 1337, Email = "jones@gmail.com" },
                    AuthorId = 1337,
                    CheepId = 1234,
                    Text = "I think therefore i am",
                    TimeStamp = new DateTime(2024, 07, 10)
                }
            );
            context.SaveChanges();
        }
    }

    public void Dispose()
    {
    }
}