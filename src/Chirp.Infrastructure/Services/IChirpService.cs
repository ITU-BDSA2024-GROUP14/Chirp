using Chirp.Infrastructure.Data.DataTransferObjects;

namespace Chirp.Infrastructure.Services;

public interface IChirpService
{
    public List<CheepDTO> GetCheeps(int page = 1);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1);
    public List<CheepDTO> GetCheepsFromMultipleAuthors(List<string> authorList, int page = 1);
    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp);
    public AuthorDTO? GetAuthorByName(string authorName);
    public AuthorDTO? GetAuthorByEmail(string authorEmail);
    public void CreateAuthor(string authorName, string authorEmail);
    public void FollowUser(string authorName, string toFollowAuthorName);
    public bool CheckIfFollowing(string authorName, string followingAuthorName);
    public void UnFollowUser(string authorName, string toUnFollowAuthorName);
    public List<string> GetFollowing(string loggedInDisplayName);
    public void RepostCheep(string authorName, int cheepId);
}