using Chirp.Core.DataModel;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public Author? GetAuthorByName(string authorName);
    public Author? GetAuthorByEmail(string authorEmail);
    public Author CreateAuthor(string authorName, string authorEmail);
}