using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestHelpers;

namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryTests : IClassFixture<ChirpDbContextFixture>
{
    private ChirpDbContextFixture _fixture;

    public AuthorRepositoryTests(ChirpDbContextFixture fixture)
    {
        _fixture = fixture;
        _fixture.Reset();
    }
}