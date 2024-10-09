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

    public Author GetAuthor(string authorName)
    {
        var query = _dbcontext.Authors.AsQueryable();
        query = query.Where(a => a.Name == authorName);
        return query.ToList().First();
    }
}