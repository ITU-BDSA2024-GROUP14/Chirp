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
        return _dbcontext.Authors.FirstOrDefault(a => a.Name == authorName);
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
        var author = new Author { Name = authorName, Email = authorEmail };
        _dbcontext.Authors.Add(author);
        _dbcontext.SaveChanges();
        return author;
    }
}