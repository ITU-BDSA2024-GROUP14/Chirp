using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Data;
using Chirp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestHelpers;

namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryTests : IClassFixture<ChirpDbContextFixture>
{
    private ChirpDbContextFixture _fixture;

    public AuthorRepositoryTests(ChirpDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.Reset();
    }

    [Fact]
    public void GetAuthorByNameTest()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var authorrepo = new AuthorRepository(context);
        var name = "Helge";
        //Act
        var author = authorrepo.GetAuthorByName(name);
        //Assert
        Assert.NotNull(author);
        Assert.Equal(name, author.Name);
    }

    [Fact]
    public void GetAuthorByEmailTest()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var authorrepo = new AuthorRepository(context);
        var name = "Helge";
        //Act
        var author = authorrepo.GetAuthorByEmail("ropf@itu.dk");
        //Assert
        Assert.NotNull(author);
        Assert.Equal(name, author.Name);
    }

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