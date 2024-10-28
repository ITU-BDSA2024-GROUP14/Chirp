using Chirp.Core.DataModel;
using Chirp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Tests;

public class AuthorRepositoryTests
{
    [Fact]
    public void AuthorRepositoryGetAuthorByName()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection).Options;
        var author = new Author { Name = "Anna", Email = "test@test.com" };
        using (var context = new ChirpDBContext(options))
        {
            context.Database.EnsureCreated();
            context.Authors.AddRange(
                new Author { Name = "Bob", Email = "john@doe.com" },
                new Author { Name = "Chalie", Email = "this@isbad.com" },
                author);
            context.SaveChanges();
        }

        using (var context = new ChirpDBContext(options))
        {
            var service = new AuthorRepository(context);
            var fromDB = service.GetAuthorByName(author.Name);

            Assert.NotNull(fromDB);
            Assert.Equal(author.Name, fromDB.Name);
            Assert.Equal(author.Email, fromDB.Email);
            Assert.Equal(author.Cheeps, fromDB.Cheeps);
        }
    }
}