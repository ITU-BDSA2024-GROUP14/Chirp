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
        var DbFace = _fixture.DbFacade;

        //Act
        var response = DbFace.GetCheeps();

        //Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response);
    }

    [Theory(Skip = "Not implemented yet")]
    [InlineData(null, null)]
    public void GetCheeps_ReturnsCheeps(string? author, int? limit)
    {
        // Arrange
        using var connection = new SqliteConnection(_fixture.Db.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """

                              """;

        // Act

        // Assert
    }
}