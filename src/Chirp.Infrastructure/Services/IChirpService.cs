using Chirp.Infrastructure.Data.DataTransferObjects;

namespace Chirp.Infrastructure.Services;

/// <summary>
///     this interface is used to define the methods that the ChirpService class will implement
/// </summary>
public interface IChirpService
{
    /// <summary>
    ///     This method is used to get a list of CheepDTO objects that correspond to the cheeps in the database
    /// </summary>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheeps(int page = 1);

    /// <summary>
    ///     This method is used to get a list of CheepDTO objects from a specific author
    ///     that correspond to the cheeps in the database.
    /// </summary>
    /// <param name="author">The author of the cheeps to get from the database</param>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1);

    /// <summary>
    ///     This method is used to get a list of CheepDTO objects from multiple authors from the database.
    /// </summary>
    /// <param name="authorList">The list of the authors you want to get cheeps from</param>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheepsFromMultipleAuthors(List<string> authorList, int page = 1);

    /// <summary>
    ///     This method is used to create a cheep in the database.
    /// </summary>
    /// <param name="authorName">The authors name of the cheep</param>
    /// <param name="authorEmail">The authors email of the cheep</param>
    /// <param name="text">The text that should appear on the cheep</param>
    /// <param name="timestamp">The timestamp the cheep was cheeped at</param>
    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp);

    /// <summary>
    ///     This method is used to get an AuthorDTO object from the database by the authors name.
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <returns>A list of AuthorDTO</returns>
    public AuthorDTO? GetAuthorByName(string authorName);

    /// <summary>
    ///     This method is used to get an AuthorDTO object from the database by the authors email.
    /// </summary>
    /// <param name="authorEmail">The authors email</param>
    /// <returns>A list of AuthorDTO</returns>
    public AuthorDTO? GetAuthorByEmail(string authorEmail);

    /// <summary>
    ///     This method is used to create an author in the database.
    /// </summary>
    /// <param name="authorName">The authors name</param>
    /// <param name="authorEmail">The authors email</param>
    public void CreateAuthor(string authorName, string authorEmail);

    /// <summary>
    ///     This method is used to follow a user in the database.
    /// </summary>
    /// <param name="authorName">The author that wants to follow another author</param>
    /// <param name="toFollowAuthorName">The author that is to be followed</param>
    public void FollowUser(string authorName, string toFollowAuthorName);

    /// <summary>
    ///     This method is used to check if an author is following another author.
    /// </summary>
    /// <param name="authorName">The name of the author that might be following someone</param>
    /// <param name="followingAuthorName">The name of the author that might be followed</param>
    /// <returns>true or false if they are following or not</returns>
    public bool CheckIfFollowing(string authorName, string followingAuthorName);

    /// <summary>
    ///     This method is used to unfollow a user in the database.
    /// </summary>
    /// <param name="authorName">The name of the author that wants to unfollow another author</param>
    /// <param name="toUnFollowAuthorName">The name of the author that is to be unfollowed</param>
    public void UnFollowUser(string authorName, string toUnFollowAuthorName);

    /// <summary>
    ///     This method is used to get a list of authors that a specific author follows.
    /// </summary>
    /// <param name="loggedInDisplayName">The name of the author from which to get the author it follows</param>
    /// <returns>A list of string corresponding to all the authors that are being followed</returns>
    public List<string> GetFollowing(string loggedInDisplayName);

    /// <summary>
    ///     This method is used to get ReCheep a cheep in the database.
    /// </summary>
    /// <param name="authorName">The author that is recheeping a cheep</param>
    /// <param name="cheepId">The id of the cheep that is to be recheeped</param>
    public void ReCheep(string authorName, int cheepId);
}