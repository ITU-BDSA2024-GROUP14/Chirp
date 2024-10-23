using Chirp.Core;
using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services;
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
}