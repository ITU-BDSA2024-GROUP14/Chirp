using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
using TestHelpers;

namespace Chirp.Infrastructure.Tests;

public class ChirpServiceTests : IClassFixture<ChirpDbContextFixture>
{
    private readonly ChirpDbContextFixture _fixture;

    public ChirpServiceTests(ChirpDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.Reset();
    }

    [Theory]
    [InlineData(1, "Starbuck now is what we hear the worst.")]
    [InlineData(2, "In the morning of the wind, some few splintered planks, of what present avail to him.")]
    public void GetCheeps_AtPage_ReturnsExpectedCheeps(int page, string expectedCheepText)
    {
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var first = service.GetCheeps(page).First();
        Assert.Equal(expectedCheepText, first.Text);
    }

    [Fact]
    public void GetCheeps_ReturnsExpectedPageSize()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var actual = service.GetCheeps().Count;
        //Assert
        Assert.Equal(ChirpService.PageSize, actual);
    }

    [Theory]
    [InlineData("Jacqualine Gilcoine")]
    [InlineData("æøå432srdf325tsdghakdhdasi hy9543ht")]
    public void GetCheepsFromAuthor_ReturnsCheeps(string expectedAuthor)
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(expectedAuthor);
        //Assert
        Assert.All(cheeps, cheep => Assert.Equal(expectedAuthor, cheep.Author));
    }

    [Fact]
    public void GetCheeps_MultipleAuthors_ReturnsCheeps()
    {
        //Arrange
        List<String> expectedAuthors = ["Jacqualine Gilcoine", "Mellie Yost"];

        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        Assert.NotEmpty(service.GetCheepsFromAuthor("Jacqualine Gilcoine"));
        Assert.NotEmpty(service.GetCheepsFromAuthor("Mellie Yost"));
        Assert.NotEmpty(service.GetCheepsFromAuthor("Quintin Sitts"));

        //Act
        var cheeps = service.GetCheepsFromMultipleAuthors(expectedAuthors);
        //Assert
        Assert.All(cheeps, cheep => Assert.Contains(expectedAuthors, author => author == cheep.Author));
        Assert.DoesNotContain(cheeps, cheep => cheep.Author == "Quintin Sitts");
    }


    [Fact]
    public void GetAuthorByName_ValidName_ReturnsAuthor()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var expected = "Helge";
        //Act
        var actual = service.GetAuthorByName("Helge");
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual.Name);
    }

    [Fact]
    public void GetAuthorByEmail_ValidEmail_ReturnsAuthor()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var expected = "Helge";
        //Act
        var actual = service.GetAuthorByEmail("ropf@itu.dk");
        //Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual.Name);
    }

    [Fact]
    public void CreateAuthor_ValidName_Succeeds()
    {
        //Arrange
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        Assert.Empty(context.Authors);

        //Act
        service.CreateAuthor("John Doe", "John@doe.com");
        //Assert
        Assert.Equal("John Doe", context.Authors.First().DisplayName);
    }

    [Fact]
    public void CreateCheep_ValidAuthor_ReturnsNewCheep()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        service.CreateCheep("Helge", text: "This is good test CHEEP", authorEmail: "rpof@itu.dk",
            timestamp: DateTime.Now);
        //Assert
        var cheep = context.Cheeps.AsEnumerable().First(cheep => cheep.GetText() == "This is good test CHEEP");
        Assert.Equal("Helge", cheep.Author.DisplayName);
    }

    [Fact]
    public void CreateCheep_NonExistentAuthor_CreatesAuthorAndReturnsNewCheep()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var authorName = "436567y78ouipoujhgv";
        //Act
        service.CreateCheep(authorName, text: "This is good test CHEEP", authorEmail: "rpof@itu.dk",
            timestamp: DateTime.Now);
        //Assert
        var cheep = context.Cheeps.AsEnumerable().First(cheep => cheep.GetText() == "This is good test CHEEP");
        var author = context.Authors.First(author => author.DisplayName == authorName);
        Assert.Equal(authorName, cheep.Author.DisplayName);
        Assert.NotNull(author);
    }

    [Theory]
    [InlineData("Helge", 1)]
    [InlineData("4567698097657", 0)]
    [InlineData("Jacqualine Gilcoine", 32)]
    public void GetCheeps_FromAuthor_ReturnsCorrectAmount(string author, int amount)
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(author);
        //Assert
        Assert.Equal(amount, cheeps.Count);
    }

    [Theory]
    [InlineData("Jacqualine Gilcoine", 32)]
    [InlineData("Helge", 0)]
    public void GetCheeps_FromAuthorAtPage_ReturnsCorrectAmount(string author, int amount)
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(author, 2);
        //Assert
        Assert.Equal(amount, cheeps.Count);
    }

    [Fact]
    public void GetCheeps_AtPage_ReturnsDistinctCheeps()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author = "Jacqualine Gilcoine";
        //Act
        var cheeps1 = service.GetCheepsFromAuthor(author);
        var cheeps2 = service.GetCheepsFromAuthor(author, 2);
        //Assert
        Assert.Empty(cheeps1.Intersect(cheeps2));
    }

    [Fact]
    public void GetAuthorByName_NonExistentName_ReturnsNull()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author = "1298Socrates";
        //Act
        var actual = service.GetAuthorByName(author);
        //Assert
        Assert.Null(actual);
    }


    [Fact]
    public void GetAuthorByEmail_NonExistentEmail_ReturnsNull()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author = "34656789@Socrates.dk";
        //Act
        var actual = service.GetAuthorByEmail(author);
        //Assert
        Assert.Null(actual);
    }

    [Fact]
    public void FollowUser_AddsToFollowingList()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);


        //Act
        service.FollowUser("Roger Histand", "Luanna Muro");
        var author1 = authorrepo.GetAuthorByName("Roger Histand");
        var author2 = authorrepo.GetAuthorByName("Luanna Muro");

        //Assert
        Assert.NotNull(author1);
        Assert.NotNull(author2);
        Assert.Contains(author1.Following, author => author == author2);
    }

    [Fact]
    public void FollowUser_FollowSelf_ThrowsException()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);


        //Act
        Assert.Throws<ArgumentException>(() => service.FollowUser("Roger Histand", "Roger Histand")
        );
        var author1 = authorrepo.GetAuthorByName("Roger Histand");

        //Assert
        Assert.NotNull(author1);
        Assert.DoesNotContain(author1.Following, author => author == author1);
    }

    [Fact]
    public void UnfollowUser_RemovesUserFromFollowingList()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author1 = authorrepo.GetAuthorByName("Roger Histand");
        var author2 = authorrepo.GetAuthorByName("Luanna Muro");
        Assert.NotNull(author1);
        Assert.NotNull(author2);
        authorrepo.FollowUser(author1, author2);
        Assert.Contains(author1.Following, author => author == author2);

        //Act
        service.UnFollowUser("Roger Histand", "Luanna Muro");

        //Assert
        Assert.DoesNotContain(author1.Following, author => author == author2);
    }

    [Fact]
    public void CheckIfFollowing_UserFollowingUser_ReturnsTrue()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author1 = authorrepo.GetAuthorByName("Roger Histand");
        var author2 = authorrepo.GetAuthorByName("Luanna Muro");
        Assert.NotNull(author1);
        Assert.NotNull(author2);
        authorrepo.FollowUser(author1, author2);
        Assert.Contains(author1.Following, author => author == author2);

        //Act
        var result = service.CheckIfFollowing("Roger Histand", "Luanna Muro");

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void GetFollowing_ReturnsFollowingList()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author1 = authorrepo.GetAuthorByName("Roger Histand");
        var author2 = authorrepo.GetAuthorByName("Luanna Muro");
        Assert.NotNull(author1);
        Assert.NotNull(author2);
        authorrepo.FollowUser(author1, author2);
        Assert.Contains(author1.Following, author => author == author2);
        Assert.Single(author1.Following);

        //Act
        var result = service.GetFollowing("Roger Histand");

        //Assert
        Assert.Contains(result, s => s == "Luanna Muro");
        Assert.Single(result);
    }

    [Fact]
    public void GetFollowing_None_ReturnsEmptyList()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        var author1 = authorrepo.GetAuthorByName("Roger Histand");
        Assert.NotNull(author1);
        Assert.Empty(author1.Following);

        //Act
        var result = service.GetFollowing("Roger Histand");

        //Assert
        Assert.Empty(result);
    }
}