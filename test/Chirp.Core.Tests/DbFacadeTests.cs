using Microsoft.Data.Sqlite;
using TestHelpers;

namespace Chirp.Core.Tests;

public class DbFacadeTests : IClassFixture<DbFacadeFixture>
{
    private readonly DbFacadeFixture _fixture;

    public DbFacadeTests(DbFacadeFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCheep_ShouldReturn()
    {
        // Arrange
        var dbFacade = _fixture.DbFacade;
        _fixture.Reset();
        _fixture.SeedTestData();


        //Act
        var response = dbFacade.GetCheeps(1);

        //Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response);
    }

    [Theory]
    [InlineData("Mellie Yost", 7)]
    [InlineData("Octavio Wagganer", 15)]
    [InlineData("Helge", 1)]
    [InlineData("Adrian", 1)]
    [InlineData("123", 0)]
    public void GetCheepsWithAuthor_ReturnsOnlyAuthorCheeps(string author, int expectedCount)
    {
        // Arrange
        _fixture.Reset();
        _fixture.SeedTestData();

        // Act
        var cheeps = _fixture.DbFacade.GetCheeps(1, author).ToList();

        // Assert
        Assert.NotNull(cheeps);
        Assert.Equal(expectedCount, cheeps.Count);
    }

    [Fact]
    public void GetCheepsWithSpecificPage_ReturnsCorrectNumberCheeps()
    {
        // Arrange
        _fixture.Reset();
        _fixture.SeedTestData();

        // Act
        var cheeps = _fixture.DbFacade.GetCheeps(1).ToList();

        // Assert
        Assert.NotNull(cheeps);
        Assert.Equal(32, cheeps.Count);
    }
}