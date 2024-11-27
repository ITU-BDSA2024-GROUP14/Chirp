using Chirp.Core;
using Chirp.Core.DataModel;
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
        var service = new ChirpService(cheeprepo, authorrepo);
        var first = service.GetCheeps(page).First();
        Assert.Equal(expectedCheepText, first.Text);
    }

    [Fact]
    public void GetCheepCorrectPageSize()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var actual = service.GetCheeps(1).Count;
        //Assert
        Assert.Equal(ChirpService.PageSize, actual);
    }

    [Theory]
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
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(expectedAuthor);
        //Assert
        Assert.All(cheeps, cheep => Assert.Equal(expectedAuthor, cheep.Author));
    }
    
    [Fact]
    public void GetCheepsFromMultipleAuthors()
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
    public void GetAuthorByNameNotNull()
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
    public void GetAuthorByNameExpectsNull()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var actual = service.GetAuthorByName("Belge");
        //Assert
        Assert.Null(actual);
    }

    [Fact]
    public void GetAuthorByEmailNotNull()
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
    public void GetAuthorByEmailExpectsNull()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var actual = service.GetAuthorByEmail("Belge");
        //Assert
        Assert.Null(actual);
    }


    [Fact]
    public void CreateAuthor()
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
    public void CreateCheepAuthorExists()
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
    public void CreateCheepAuthorDosentExist()
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
    public void GetCorrectAmountOfCheeps(string author, int amount)
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
    public void GetCorrectAmountOfCheepsPage2(string author, int amount)
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var cheeprepo = new CheepRepository(context);
        var authorrepo = new AuthorRepository(context);
        var service = new ChirpService(cheeprepo, authorrepo);
        //Act
        var cheeps = service.GetCheepsFromAuthor(author,2);
        //Assert
        Assert.Equal(amount, cheeps.Count);
    }

    [Fact]
    public void NoDupilicateCheepsOnDifferentPages()
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
        var cheeps1 = service.GetCheepsFromAuthor(author, 1);
        var cheeps2 = service.GetCheepsFromAuthor(author,2);
        //Assert
        Assert.Empty(cheeps1.Intersect(cheeps2));
    }

    [Fact]
    public void GetAuthorByNameThatDosentExistExpectsNull()
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
    public void GetAuthorByEmailThatDosentExistExpectsNull()
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
    public void FollowUser()
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
        Assert.Contains(author1.Following, author => author==author2);
    }
    
    [Fact]
    public void FollowUserCannotFollowSelf()
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
        Assert.DoesNotContain(author1.Following, author => author==author1);
    }
    
    [Fact]
    public void UnFollowUser()
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
        Assert.Contains(author1.Following, author => author==author2);
        
        //Act
        service.UnFollowUser("Roger Histand", "Luanna Muro");
        
        //Assert
        Assert.DoesNotContain(author1.Following, author => author==author2);
    }

    [Fact]
    public void CheckIfFollowingReturnsTrue()
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
        Assert.Contains(author1.Following, author => author==author2);
        
        //Act
        bool result = service.CheckIfFollowing("Roger Histand", "Luanna Muro");
        
        //Assert
        Assert.True(result);
        
    }
    
    [Fact]
    public void GetFollowingReturnsFollower()
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
        Assert.Contains(author1.Following, author => author==author2);
        Assert.Single(author1.Following);
        
        //Act
        List<String> result = service.GetFollowing("Roger Histand");
        
        //Assert
        Assert.Contains(result, s => s=="Luanna Muro");
        Assert.Single(result);
    }
    
    [Fact]
    public void GetFollowingReturnsEmpty()
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
        List<String> result = service.GetFollowing("Roger Histand");
        
        //Assert
        Assert.Empty(result);
    }
    
}