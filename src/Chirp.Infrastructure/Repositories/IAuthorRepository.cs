using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Repositories;

public interface IAuthorRepository
{
    public Author? GetAuthorByName(string authorName);
    public Author? GetAuthorByEmail(string authorEmail);
    public Author CreateAuthor(string authorName, string authorEmail);
    public void FollowUser(Author user, Author toFollowAuthor);
    public List<string> GetFollowing(string authorName);
    void UnFollowUser(Author user, Author toUnFollow);
}