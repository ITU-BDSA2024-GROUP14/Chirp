using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

/// <summary>
/// Provides methods for interacting with the Author entities in the database.
/// Implements the <c>IAuthorRepository</c> interface.
/// </summary>
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
        return _dbcontext.Authors.FirstOrDefault(a => a.DisplayName == authorName);
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
        var author = new Author { DisplayName = authorName, Email = authorEmail };
        _dbcontext.Authors.Add(author);
        _dbcontext.SaveChanges();
        return author;
    }

    /// <summary>
    /// Make an author follow another author.
    /// </summary>
    /// <param name="user">The author who will follow another author.</param>
    /// <param name="toFollowAuthor">The author to be followed.</param>
    /// <exception cref="AuthorMissingException">Thrown when either the user or the target author does not exist.</exception>
    public void FollowUser(Author user, Author toFollowAuthor)
    {
        if (!Exists(user))
        {
            throw new AuthorMissingException(user.DisplayName);
        }

        if (!Exists(toFollowAuthor))
        {
            throw new AuthorMissingException(user.DisplayName);
        }
        
        if (IsFollowing(user, toFollowAuthor))
        {
            return;
        }

        _dbcontext.Authors.Update(user);
        user.Following.Add(toFollowAuthor);
        _dbcontext.SaveChanges();
    }

    /// <summary>
    /// Stops the specified user from following another user.
    /// </summary>
    /// <param name="user">The user attempting to unfollow another user.</param>
    /// <param name="toUnFollow">The user to be unfollowed by the original user.</param>
    /// <exception cref="AuthorMissingException">Thrown when either the user or the user to unfollow does not exist.</exception>
    public void UnFollowUser(Author user, Author toUnFollow)
    {
        if (!Exists(user))
        {
            throw new AuthorMissingException(user.DisplayName);
        }
        
        if (!Exists(toUnFollow))
        {
            throw new AuthorMissingException(user.DisplayName);
        }
        
        if (!IsFollowing(user, toUnFollow))
        {
            return;
        }

        _dbcontext.Authors.Update(user);
        user.Following.Remove(toUnFollow);
        _dbcontext.SaveChanges();
    }

    /// <summary>
    /// Determines if the given user is currently following the specified author.
    /// </summary>
    /// <param name="user">The user checking their following status.</param>
    /// <param name="author">The author to check if they are being followed.</param>
    /// <returns>True if the user is following the specified author; otherwise, false.</returns>
    private static bool IsFollowing(Author user, Author author)
    {
        return user.Following.Contains(author);
    }

    /// <summary>
    /// Determines if the specified author exists in the database.
    /// </summary>
    /// <param name="author">The author to check.</param>
    /// <returns><c>true</c> if the author exists; otherwise, <c>false</c>.</returns>
    private bool Exists(Author author)
    {
        return _dbcontext.Authors.Any(a => a.DisplayName == author.DisplayName);
    }

    /// <summary>
    /// Retrieves a list of authors that a specified author is following.
    /// </summary>
    /// <param name="authorName">The display name of the author.</param>
    /// <returns>A list of display names of the authors being followed by the specified author.</returns>
    public List<string> GetFollowing(string authorName)
    {
        var author = _dbcontext.Authors.Include(author => author.Following).First(author => author.DisplayName == authorName);
        return author.Following.Select(a => a.DisplayName).ToList();
    }
}