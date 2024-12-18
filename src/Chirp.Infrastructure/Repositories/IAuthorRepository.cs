using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Repositories;

/// <summary>
///     Defines the repository operations related to the <c>Author</c> entity.
///     Provides methods for retrieving, creating, and managing <c>Author</c> entities and their relationships.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    ///     Retrieve an author by their display name.
    /// </summary>
    /// <param name="authorName">The display name of the author to be retrieved.</param>
    /// <returns>
    ///     The <c>Author</c> object that matches the given display name,
    ///     or <c>null</c> if no such author exists.
    /// </returns>
    public Author? GetAuthorByName(string authorName);

    /// <summary>
    ///     Retrieve an author by their email address.
    /// </summary>
    /// <param name="authorEmail">The email address of the author to be retrieved.</param>
    /// <returns>
    ///     The <c>Author</c> object that matches the given email address,
    ///     or <c>null</c> if no such author exists.
    /// </returns>
    public Author? GetAuthorByEmail(string authorEmail);

    /// <summary>
    ///     Create a new author with the specified name and email address and adds it to the repository.
    /// </summary>
    /// <param name="authorName">The display name of the author to be created.</param>
    /// <param name="authorEmail">The email address of the author to be created.</param>
    /// <returns>
    ///     The <c>Author</c> object representing the created author.
    /// </returns>
    public Author CreateAuthor(string authorName, string authorEmail);

    /// <summary>
    ///     Make an author follow another author.
    /// </summary>
    /// <param name="user">The author performing the follow action.</param>
    /// <param name="toFollowAuthor">The author to be followed.</param>
    public void FollowUser(Author user, Author toFollowAuthor);

    /// <summary>
    ///     Retrieve a list of authors that the specified author is following.
    /// </summary>
    /// <param name="authorName">The display name of the author whose following list is to be retrieved.</param>
    /// <returns>
    ///     A list of display names of the authors that the specified author is following.
    /// </returns>
    public List<string> GetFollowing(string authorName);

    /// <summary>
    ///     Stops a user from following another specified user.
    /// </summary>
    /// <param name="user">The user who is performing the unfollow action.</param>
    /// <param name="toUnFollow">The user to be unfollowed.</param>
    void UnFollowUser(Author user, Author toUnFollow);
}