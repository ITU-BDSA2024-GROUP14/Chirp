using Chirp.Core.DataModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestHelpers;

namespace Chirp.Core.Tests;

public class CheepRepositoryTests : IClassFixture<CheepRepositoryFixture>
{
    private CheepRepositoryFixture _fixture;

    public CheepRepositoryTests(CheepRepositoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("jones", 1337, "jones@gmail.com", 1234, "I think therefore i am", 2024, 07, 10)]
    public void CheepRepositoryReturns(string authorName, int authorId, string email, int cheepId, string text,
        int year, int month, int day)
    {
        var author = new Author { Name = authorName, AuthorId = authorId, Email = email };
        var date = new DateTime(year, month, day);

        using (var context = _fixture.CdbContext)
        {
            context.Database.EnsureCreated();
            context.Cheeps.AddRange(
                new Cheep
                {
                    Author = author,
                    AuthorId = author.AuthorId,
                    CheepId = cheepId,
                    Text = text,
                    TimeStamp = date
                }
            );
            context.SaveChanges();
        }

        using (var context = _fixture.CdbContext)
        {
            var service = new CheepRepository(context);
            var Cheeps = service.GetCheeps();

            Assert.NotEmpty(Cheeps);
            var cheep = Cheeps.First();

            Assert.Equal(authorId, cheep.AuthorId);
            Assert.Equal(authorName, cheep.Author.Name);
            Assert.Equal(cheepId, cheep.CheepId);
            Assert.Equal(text, cheep.Text);
            Assert.Equal(date, cheep.TimeStamp);
        }
    }

    [Fact]
    public void SeedsDefaultData()
    {
        _fixture.SeedDatabase();

        using (var context = _fixture.CdbContext)
        {
            context.Database.EnsureCreated();
            var service = new CheepRepository(context);
            Assert.NotEmpty(service.GetCheeps());
        }
    }

    [Fact]
    public void DatabaseStartsEmpty()
    {
        using (var context = _fixture.CdbContext)
        {
            context.Database.EnsureCreated();
            var service = new CheepRepository(context);
            Assert.Empty(service.GetCheeps());
        }
    }
}