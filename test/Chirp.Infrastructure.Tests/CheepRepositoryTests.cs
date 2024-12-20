using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Repositories;
using TestHelpers;

namespace Chirp.Infrastructure.Tests;

/// <summary>
///     Tests for the CheepRepository.
/// </summary>
public class CheepRepositoryTests : IClassFixture<ChirpDbContextFixture>
{
    private readonly ChirpDbContextFixture _fixture;

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
            expectedCount = context.Cheeps.Count() + 1;

            var author = new Author { DisplayName = authorName, AuthorId = authorId, Email = email };
            var date = new DateTime(year, month, day);

            context.Cheeps.Add(
                new OriginalCheep
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
            c.GetText() == text &&
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
    public void CreateCheep_TooLong_ThrowsException()
    {
        var author = new Author { DisplayName = "jones", AuthorId = 1234, Email = "jones@mail.com" };
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
                Assert.InRange(cheeps.First().GetText().Length, 0, Cheep.MaxLength);
            }
        }
    }

    [Fact]
    public void GetCheeps_OrderedByCreateDate()
    {
        // Arrange
        var author = new Author { DisplayName = "jones", AuthorId = 1234, Email = "jones@mail.com" };

        using (var context = _fixture.CreateContext())
        {
            var repository = new CheepRepository(context);
            repository.CreateCheep(author, "short cheep", new DateTime(2024, 03, 02));
            repository.CreateCheep(author, "short cheep", new DateTime(2024, 03, 01));
            repository.CreateCheep(author, "short cheep", new DateTime(2024, 03, 03));
        }

        // Act
        using (var context = _fixture.CreateContext())
        {
            var repository = new CheepRepository(context);
            var cheeps = repository.GetCheeps().ToList();
            // Assert
            Assert.Equal(3, cheeps.Count);
            Assert.Equal(new DateTime(2024, 03, 03), cheeps[0].TimeStamp);
            Assert.Equal(new DateTime(2024, 03, 02), cheeps[1].TimeStamp);
            Assert.Equal(new DateTime(2024, 03, 01), cheeps[2].TimeStamp);
        }
    }
}