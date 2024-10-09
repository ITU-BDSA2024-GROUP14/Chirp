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

    [Fact]
    public void CheepRepositoryReturns()
    {
        using (var context = new ChirpDBContext(_fixture.Options))
        {
            var service = new CheepRepository(context);
            var Cheeps = service.GetCheeps();

            Assert.NotEmpty(Cheeps);
            var cheep = Cheeps.First();

            Assert.Equal(1337, cheep.AuthorId);
            Assert.Equal("jones", cheep.Author.Name);
            Assert.Equal(1234, cheep.CheepId);
            Assert.Equal("I think therefore i am", cheep.Text);
            Assert.Equal(new DateTime(2024, 07, 10).Date, cheep.TimeStamp);
        }
    }
}