using Chirp.Core;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Storage;
using TestHelpers;

namespace Chirp.Infrastructure.Tests;

public class ChirpServiceTests : IClassFixture<ChirpDbContextFixture>
{
    private ChirpDbContextFixture _fixture;

    public ChirpServiceTests(ChirpDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.Reset();
    }

    [Theory]
    [InlineData(1, "Starbuck now is what we hear the worst.")]
    [InlineData(2, "In the morning of the wind, some few splintered planks, of what present avail to him.")]
    public void GetCheepFromPage(int page, string expectedCheepText)
    {
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheepRepository: cheeprepo, authorRepository: authorrepo);
        var first = service.GetCheeps(page).First();
        Assert.Equal(expectedCheepText, first.Text);
    }

    [Fact]
    public void GetCheepCorrectPageSize(){
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheepRepository: cheeprepo, authorRepository: authorrepo);
        //Act
        var actual = service.GetCheeps(1).Count;
        //Assert
        Assert.Equal(ChirpService.PageSize, actual);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Jacqualine Gilcoine")]
    [InlineData("æøå432srdf325tsdghakdhdasi hy9543ht")]
    public void GetCheepByAuthor(string expectedAuthor)
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheepRepository: cheeprepo, authorRepository: authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(expectedAuthor);
        //Assert
        Assert.All(cheeps, cheep => Assert.Equal(expectedAuthor, cheep.Author));
    }

    [Fact]
    public void GetAuthorByNameNotNull()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheepRepository: cheeprepo, authorRepository: authorrepo);
        var expected = "Helge";
        //Act
        var actual = service.GetAuthorByName("Helge");
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual.Name);
    }

    [Fact]
    public void GetAuthorByNameExpectsNull()
    {

        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheepRepository: cheeprepo, authorRepository: authorrepo);
        //Act
        var actual = service.GetAuthorByName("Belge");
        //Assert
        Assert.Null(actual);
    }
}