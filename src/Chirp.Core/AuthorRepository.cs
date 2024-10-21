using Chirp.Core.DataModel;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public Author? GetAuthorByName(string authorName);
    public Author? GetAuthorByEmail(string authorEmail);
    public Author CreateAuthor(string authorName, string authorEmail);
}

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
        return GetAuthor(authorName);
    }

    /// <summary>
    /// Get an author from the database by email
    /// </summary>
    /// <param name="authorEmail">The email of the author</param>
    /// <returns>The requested author</returns>
    public Author? GetAuthorByEmail(string authorEmail)
    {
        return GetAuthor(authorEmail: authorEmail);
    }

    public Author? GetAuthor(string? authorName = null, string? authorEmail = null)
    {
        if (authorName == null && authorEmail == null)
        {
            throw new ArgumentException("Please supply at least one argument that isnt null");
        }

        var query = _dbcontext.Authors.AsQueryable();
        if (authorName != null)
        {
            query = query.Where(a => a.Name == authorName);
        }

        if (authorEmail != null)
        {
            query = query.Where(author => author.Email == authorEmail);
        }

        return query.FirstOrDefault();
    }

    /// <summary>
    /// Instantiates an Author and adds it to the database.
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <param name="authorEmail">The email of the author</param>
    public Author CreateAuthor(string authorName, string authorEmail)
    {
        var author = new Author { Name = authorName, Email = authorEmail };
        _dbcontext.Authors.Add(author);
        _dbcontext.SaveChanges();
        return author;
    }
}