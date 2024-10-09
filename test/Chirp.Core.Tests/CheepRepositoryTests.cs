using Chirp.Core.DataModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Tests;

/// <summary>
/// Tests for the CheepRepository.
/// </summary>
public class CheepRepositoryTests
{
    [Fact]
    public void CheepRepositoryReturns()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection).Options;

        using (var context = new ChirpDBContext(options))
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

        using (var context = new ChirpDBContext(options))
        {
            var service = new CheepRepository(context);
            var Cheeps = service.GetCheeps();

            Assert.NotEmpty(Cheeps);
            var cheep = Cheeps.First();

            Assert.Equal(1337, cheep.AuthorId);
            Assert.Equal("jones", cheep.Author.Name);
            Assert.Equal(1234, cheep.CheepId);
            Assert.Equal("I think therefore i am", cheep.Text);
            Assert.Equal(new DateTime(2024, 07, 10).Date, cheep.TimeStamp);
        }
    }
}