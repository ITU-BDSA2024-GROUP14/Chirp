using Chirp.Core;
using Chirp.Core.DataTransferObjects;

namespace Chirp.Razor;

public interface IAuthorService
{
    public AuthorDTO GetAuthor(string? authorName = null, string? authorEmail = null);
}

public class AuthorService : IAuthorService
{
    private IAuthorRepository _database;

    public AuthorService(IAuthorRepository db)
    {
        _database = db;
    }

    public AuthorDTO GetAuthor(string? authorName = null, string? authorEmail = null)
    {
        var author = _database.GetAuthor(authorName: authorName, authorEmail: authorEmail);
        var dto = new AuthorDTO {Name = author.Name, Email = author.Email};
        return dto;
    }
}