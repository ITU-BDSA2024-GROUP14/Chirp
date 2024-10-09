using Chirp.Core.DataModel;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public Author GetAuthor(string? authorName = null, string? authorEmail = null);
}

public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext _dbcontext;

    public AuthorRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    /// <summary>
    /// Get an author from the database
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <param name="authorEmail">The email of the author</param>
    /// <returns>The requested author</returns>
    public Author GetAuthor(string? authorName = null, string? authorEmail = null)
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
        return query.ToList().First();
    }
}