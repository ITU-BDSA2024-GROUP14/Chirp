using Chirp.Core.DataModel;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public Author GetAuthor(string authorName);
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
    /// <returns>The requested author</returns>
    public Author GetAuthor(string authorName)
    {
        var query = _dbcontext.Authors.AsQueryable();
        query = query.Where(a => a.Name == authorName);
        return query.ToList().First();
    }
}