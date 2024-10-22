using Chirp.Core.DataModel;
using TestHelpers;

namespace Chirp.Core.Tests;

/// <summary>
/// Tests for the CheepRepository.
/// </summary>
public class CheepRepositoryTests : IClassFixture<ChirpDbContextFixture>
{
    private ChirpDbContextFixture _fixture;

    public CheepRepositoryTests(ChirpDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.Reset();
    }

    [Theory]
    [InlineData("ones", 1337, "jones@gmail.com", 1234, "I think therefore i am", 2024, 07, 10)]
    public void GetCheeps_ReturnsAllCheeps(string authorName, int authorId, string email, int cheepId, string text,
        int year, int month, int day)
    {
        // Arrange
        _fixture.SeedDatabase();
        int expectedCount;
        using (var context = _fixture.CreateContext())
        {
            var author = new Author { Name = authorName, AuthorId = authorId, Email = email };
            var date = new DateTime(year, month, day);

            context.Cheeps.Add(
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
            expectedCount = context.Cheeps.Count();
        }

        // Act
        List<Cheep> cheepsReturned;
        using (var context = _fixture.CreateContext())
        {
            var service = new CheepRepository(context);
            cheepsReturned = service.GetCheeps().ToList();
        }

        // Assert
        Assert.NotEmpty(cheepsReturned);
        Assert.Equal(expectedCount, cheepsReturned.Count);
        // Check that the added cheep is in the list
        Assert.Contains(cheepsReturned, c =>
            c.Text == text &&
            c.AuthorId == authorId &&
            c.CheepId == cheepId &&
            c.TimeStamp.Year == year &&
            c.TimeStamp.Month == month &&
            c.TimeStamp.Day == day);
    }

    [Fact]
    public void SeedsDefaultData()
    {
        _fixture.SeedDatabase();

        using (var context = _fixture.CreateContext())
        {
            context.Database.EnsureCreated();
            var service = new CheepRepository(context);
            Assert.NotEmpty(service.GetCheeps());
        }
    }

    [Fact]
    public void DatabaseStartsEmpty()
    {
        using (var context = _fixture.CreateContext())
        {
            context.Database.EnsureCreated();
            var service = new CheepRepository(context);
            Assert.Empty(service.GetCheeps());
        }
    }

    [Fact] //Tests that a 190 character long cheep text is handled correctly
    public void LongCheepsDisallowed()
    {
        var author = new Author { Name = "jones", AuthorId = 1234, Email = "jones@mail.com" };
        var date = new DateTime(2024, 03, 02);
        using (var context = _fixture.CreateContext())
        {
            var repository = new CheepRepository(context);
            Assert.Throws<CheepTooLongException>(() =>
                repository.CreateCheep(author,
                    "If i were to one day write a cheep, that should be very long, it would certainly have some content. There is no way that I could write a long cheep without actually conveying any information.",
                    date));
        }

        using (var context = _fixture.CreateContext())
        {
            context.Database.EnsureCreated();
            var service = new CheepRepository(context);
            var cheeps = service.GetCheeps();
            if (cheeps.Any())
            {
                Assert.InRange(cheeps.First().Text.Length, 0, Cheep.MaxLength);
            }
        }
    }
}