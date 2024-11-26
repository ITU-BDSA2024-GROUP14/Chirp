using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext _dbcontext;

    public AuthorRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    /// <summary>
    /// Get an author from the database by name
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <returns>The requested author</returns>
    public Author? GetAuthorByName(string authorName)
    {
        return _dbcontext.Authors.FirstOrDefault(a => a.Beak == authorName);
    }

    /// <summary>
    /// Get an author from the database by email
    /// </summary>
    /// <param name="authorEmail">The email of the author</param>
    /// <returns>The requested author</returns>
    public Author? GetAuthorByEmail(string authorEmail)
    {
        return _dbcontext.Authors.FirstOrDefault(a => a.Email == authorEmail);
    }

    /// <summary>
    /// Instantiates an Author and adds it to the database.
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <param name="authorEmail">The email of the author</param>
    public Author CreateAuthor(string authorName, string authorEmail)
    {
        var author = new Author { Beak = authorName, Email = authorEmail };
        _dbcontext.Authors.Add(author);
        _dbcontext.SaveChanges();
        return author;
    }

    public void FollowUser(Author user, Author toFollowAuthor)
    {
        if (!Exists(user))
        {
            throw new AuthorMissingException(user.Beak);
        }
        
        if (!Exists(toFollowAuthor))
        {
            throw new AuthorMissingException(user.Beak);
        }
        
        if (IsFollowing(user, toFollowAuthor))
        {
            return;
        }

        _dbcontext.Authors.Update(user);
        user.Following.Add(toFollowAuthor);
        _dbcontext.SaveChanges();
    }

    public void UnFollowUser(Author user, Author toUnFollow)
    {
        if (!Exists(user))
        {
            throw new AuthorMissingException(user.Beak);
        }
        
        if (!Exists(toUnFollow))
        {
            throw new AuthorMissingException(user.Beak);
        }
        
        if (!IsFollowing(user, toUnFollow))
        {
            return;
        }

        _dbcontext.Authors.Update(user);
        user.Following.Remove(toUnFollow);
        _dbcontext.SaveChanges();
    }
    
    private static bool IsFollowing(Author user, Author author)
    {
        return user.Following.Contains(author);
    }

    private bool Exists(Author author)
    {
        return _dbcontext.Authors.Any(a => a.Beak == author.Beak);
    }

    public List<string> GetFollowing(string authorName)
    {
        var author = _dbcontext.Authors.Include(author => author.Following).First(author => author.Beak == authorName);
        return author.Following.Select(a => a.Beak).ToList();
    }
}