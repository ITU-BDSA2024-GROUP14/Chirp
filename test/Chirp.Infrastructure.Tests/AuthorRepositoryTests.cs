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
    public void GetAuthorByName_ReturnsAuthor()
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
        Assert.Equal(name, author.Beak);
    }

    [Fact]
    public void GetAuthorByEmail_ReturnsAuthor()
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
        Assert.Equal(name, author.Beak);
    }

    [Fact]
    public void AuthorRepositoryGetAuthorByName()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection).Options;
        var author = new Author { Beak = "Anna", Email = "test@test.com" };
        using (var context = new ChirpDBContext(options))
        {
            context.Database.EnsureCreated();
            context.Authors.AddRange(
                new Author { Beak = "Bob", Email = "john@doe.com" },
                new Author { Beak = "Chalie", Email = "this@isbad.com" },
                author);
            context.SaveChanges();
        }

        using (var context = new ChirpDBContext(options))
        {
            var service = new AuthorRepository(context);
            var fromDB = service.GetAuthorByName(author.Beak);

            Assert.NotNull(fromDB);
            Assert.Equal(author.Beak, fromDB.Beak);
            Assert.Equal(author.Email, fromDB.Email);
            Assert.Equal(author.Cheeps, fromDB.Cheeps);
        }
    }

    [Fact]
    public void UserFollow_User_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Morten", Email = "morten@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.Beak == followee.Beak);

            Assert.Contains(updatedFollowee, updatedFollower.Following);
            Assert.DoesNotContain(updatedFollower, updatedFollowee.Following);
        }
    }

    [Fact]
    public void UserFollow_UserAlreadyFollowing_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Morten", Email = "morten@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.Beak == followee.Beak);

            Assert.Contains(updatedFollowee, updatedFollower.Following);
            Assert.DoesNotContain(updatedFollower, updatedFollowee.Following);
        }
    }

    [Fact]
    public void UserFollow_MissingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);

            Assert.Empty(updatedFollower.Following);
            Assert.False(context.Authors.Any(a => a.Beak == followee.Beak));
        }
    }

    [Fact]
    public void MissingUserFollow_ExistingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

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
            Assert.False(context.Authors.Any(a => a.Beak == follower.Beak));

            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.Beak == followee.Beak);

            Assert.Empty(updatedFollowee.Following);
        }
    }

    [Fact]
    public void MissingUserFollow_MissingUser_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.FollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            Assert.False(context.Authors.Any(a => a.Beak == follower.Beak));
            Assert.False(context.Authors.Any(a => a.Beak == followee.Beak));
        }
    }

    [Fact]
    public void UserUnfollow_User_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.Beak == followee.Beak);

            Assert.DoesNotContain(updatedFollowee, updatedFollower.Following);
            Assert.Empty(updatedFollowee.Following);
        }
    }

    [Fact]
    public void MissingUserUnfollow_MissingUser_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

        using (var context = _fixture.CreateContext())
        {
            // Act
            var authorRepo = new AuthorRepository(context);
            Assert.Throws<AuthorMissingException>(() => authorRepo.UnFollowUser(follower, followee));
        }

        // Assert
        using (var context = _fixture.CreateContext())
        {
            Assert.False(context.Authors.Any(a => a.Beak == follower.Beak));
            Assert.False(context.Authors.Any(a => a.Beak == followee.Beak));
        }
    }

    [Fact]
    public void UserUnfollow_NotFollowingUser_Succeeds()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };
        var followee = new Author { Beak = "Jonathan", Email = "jonathan@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);
            var updatedFollowee = context.Authors.Include(a => a.Following).First(a => a.Beak == followee.Beak);

            Assert.Empty(updatedFollowee.Following);
            Assert.Empty(updatedFollower.Following);
        }
    }

    [Fact]
    public void UserFollow_Self_Fails()
    {
        // Arrange
        _fixture.SeedDatabase();

        var follower = new Author { Beak = "Anna", Email = "anna@test.com" };

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
            var updatedFollower = context.Authors.Include(a => a.Following).First(a => a.Beak == follower.Beak);
            Assert.Empty(updatedFollower.Following);
        }
    }
}