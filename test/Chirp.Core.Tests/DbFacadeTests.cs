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
        var response = dbFacade.GetCheeps();

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
        var cheeps = _fixture.DbFacade.GetCheeps(author).ToList();

        // Assert
        Assert.NotNull(cheeps);
        Assert.Equal(expectedCount, cheeps.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void GetCheepsWithLimit_ReturnsCorrectCount(int limit)
    {
        // Arrange
        _fixture.Reset();
        _fixture.SeedTestData();

        // Act
        var cheeps = _fixture.DbFacade.GetCheeps(limit: limit).ToList();

        // Assert
        Assert.NotNull(cheeps);
        Assert.Equal(limit, cheeps.Count);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(-1)]
    public void GetCheepsWithInvalidLimit_ReturnsAllCheeps(int limit)
    {
        // Arrange
        _fixture.Reset();
        _fixture.SeedTestData();
        var expectedCount = _fixture.GetCount();

        // Act
        var cheeps = _fixture.DbFacade.GetCheeps(limit: limit).ToList();

        // Assert
        Assert.NotNull(cheeps);
        Assert.Equal(expectedCount, cheeps.Count);
    }
}