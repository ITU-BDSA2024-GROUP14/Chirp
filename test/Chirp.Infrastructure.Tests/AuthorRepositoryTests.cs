using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Data;
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

    [Fact]
    public void GetAuthorByNameTest()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var authorrepo = new AuthorRepository(context);
        var name = "Helge";
        //Act
        var author = authorrepo.GetAuthorByName(name);
        //Assert
        Assert.NotNull(author);
        Assert.Equal(name, author.DisplayName);
    }

    [Fact]
    public void GetAuthorByEmailTest()
    {
        //Arrange
        _fixture.SeedDatabase();
        using var context = _fixture.CreateContext();
        context.Database.EnsureCreated();
        var authorrepo = new AuthorRepository(context);
        var name = "Helge";
        //Act
        var author = authorrepo.GetAuthorByEmail("ropf@itu.dk");
        //Assert
        Assert.NotNull(author);
        Assert.Equal(name, author.DisplayName);
    }

    [Fact]
    public void AuthorRepositoryGetAuthorByName()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection).Options;
        var author = new Author { DisplayName = "Anna", Email = "test@test.com" };
        using (var context = new ChirpDBContext(options))
        {
            context.Database.EnsureCreated();
            context.Authors.AddRange(
                new Author { DisplayName = "Bob", Email = "john@doe.com" },
                new Author { DisplayName = "Chalie", Email = "this@isbad.com" },
                author);
            context.SaveChanges();
        }

        using (var context = new ChirpDBContext(options))
        {
            var service = new AuthorRepository(context);
            var fromDB = service.GetAuthorByName(author.DisplayName);

            Assert.NotNull(fromDB);
            Assert.Equal(author.DisplayName, fromDB.DisplayName);
            Assert.Equal(author.Email, fromDB.Email);
            Assert.Equal(author.Cheeps, fromDB.Cheeps);
        }
    }

    [Fact]
    public void UserFollow_User_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Morten", Email = "morten@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.AddRange(follower, followee);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            authorRepo.FollowUser(follower, followee);
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.DisplayName == followee.DisplayName);

            Assert.Contains(updatedFollowee, updatedFollower.Following);
            Assert.DoesNotContain(updatedFollower, updatedFollowee.Following);
        }
    }

    [Fact]
    public void UserFollow_UserAlreadyFollowing_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Morten", Email = "morten@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.AddRange(follower, followee);
            follower.Following.Add(followee);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            authorRepo.FollowUser(follower, followee);
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.DisplayName == followee.DisplayName);

            Assert.Contains(updatedFollowee, updatedFollower.Following);
            Assert.DoesNotContain(updatedFollower, updatedFollowee.Following);
        }
    }

    [Fact]
    public void UserFollow_MissingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.Add(follower);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.FollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);

            Assert.Empty(updatedFollower.Following);
            Assert.False(context.Authors.Any(a => a.DisplayName == followee.DisplayName));
        }
    }

    [Fact]
    public void MissingUserFollow_ExistingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.Add(followee);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.FollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            Assert.False(context.Authors.Any(a => a.DisplayName == follower.DisplayName));

            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.DisplayName == followee.DisplayName);

            Assert.Empty(updatedFollowee.Following);
        }
    }

    [Fact]
    public void MissingUserFollow_MissingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.FollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            Assert.False(context.Authors.Any(a => a.DisplayName == follower.DisplayName));
            Assert.False(context.Authors.Any(a => a.DisplayName == followee.DisplayName));
        }
    }

    [Fact]
    public void UserUnfollow_User_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.AddRange(follower, followee);
            follower.Following.Add(followee);
            context.SaveChanges();

            // Act
            Assert.Contains(followee, follower.Following);
            var authorRepo = new AuthorRepository(context);
            authorRepo.UnFollowUser(follower, followee);
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.DisplayName == followee.DisplayName);

            Assert.DoesNotContain(updatedFollowee, updatedFollower.Following);
            Assert.Empty(updatedFollowee.Following);
        }
    }

    [Fact]
    public void MissingUserUnfollow_MissingUser_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.UnFollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            Assert.False(context.Authors.Any(a => a.DisplayName == follower.DisplayName));
            Assert.False(context.Authors.Any(a => a.DisplayName == followee.DisplayName));
        }
    }

    [Fact]
    public void UserUnfollow_NotFollowingUser_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };
        var followee = new Author { DisplayName = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.AddRange(follower, followee);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            authorRepo.UnFollowUser(follower, followee);
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.DisplayName == followee.DisplayName);

            Assert.Empty(updatedFollowee.Following);
            Assert.Empty(updatedFollower.Following);
        }
    }

    [Fact]
    public void UserFollowSelf_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { DisplayName = "Anna", Email = "anna@test.com" };

        using (var context = _fixture.CreateContext())
        {
            context.Authors.AddRange(follower);
            context.SaveChanges();

            // Act
            var authorRepo = new AuthorRepository(context);
            authorRepo.UnFollowUser(follower, follower);
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.DisplayName == follower.DisplayName);
            Assert.Empty(updatedFollower.Following);
        }
    }
}