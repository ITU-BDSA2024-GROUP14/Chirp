using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Repositories;

/// <summary>
/// Defines the repository operations related to the <c>Cheep</c> entity.
/// Provides methods for retrieving and creating <c>Cheep</c> entities and their relationships.
/// </summary>
public interface ICheepRepository
{
    /// <summary>
    /// Retrieve a collection of <c>Cheep</c> entities authored by the specified list of authors.
    /// </summary>
    /// <param name="authorUsernameList">
    /// A list of display names corresponding to the authors whose <c>Cheep</c>s should be retrieved.
    /// </param>
    /// <param name="skip">
    /// The number of <c>Cheep</c>s to skip before returning results (used for pagination).
    /// </param>
    /// <param name="size">
    /// The maximum number of <c>Cheep</c>s to retrieve. If null, retrieves all remaining items.
    /// </param>
    /// <returns>
    /// A collection of <c>Cheep</c> entities satisfying the specified criteria.
    /// </returns>
    public IEnumerable<Cheep> GetCheepsByAuthor(List<string> authorUsernameList, int skip, int? size);

    /// <summary>
    /// Retrieve a collection of <c>Cheep</c> entities from the database based on the specified pagination parameters.
    /// </summary>
    /// <param name="skip">
    /// The number of <c>Cheep</c> entities to skip from the start of the result set.
    /// </param>
    /// <param name="size">
    /// The maximum number of <c>Cheep</c> entities to retrieve. If null, retrieves all remaining entities.
    /// </param>
    /// <returns>
    /// A collection of <c>Cheep</c> entities ordered by timestamp in descending order.
    /// </returns>
    public IEnumerable<Cheep> GetCheeps(int skip, int? size);

    /// <summary>
    /// Create a new <c>OriginalCheep</c> entity authored by the provided author with the specified text and timestamp.
    /// </summary>
    /// <param name="author">
    /// The <c>Author</c> who is creating the <c>Cheep</c>. Contains details such as the author's ID, display name, and email.
    /// </param>
    /// <param name="text">
    /// The content of the <c>Cheep</c>. This represents the main message being posted.
    /// </param>
    /// <param name="timestamp">
    /// The date and time when the <c>Cheep</c> is created.
    /// </param>
    /// <returns>
    /// The newly created <c>OriginalCheep</c> entity.
    /// </returns>
    public OriginalCheep CreateCheep(Author author, string text, DateTime timestamp);

    /// <summary>
    /// Create a new reposted <c>Cheep</c> (ReCheep) by an author referencing an existing original <c>Cheep</c>.
    /// </summary>
    /// <param name="author">
    /// The author creating the reposted <c>Cheep</c>.
    /// </param>
    /// <param name="originalCheep">
    /// The original <c>Cheep</c> being reposted.
    /// </param>
    /// <param name="timestamp">
    /// The date and time when the reposted <c>Cheep</c> is created.
    /// </param>
    /// <returns>
    /// A new <c>RepostCheep</c> entity representing the reposted <c>Cheep</c>.
    /// </returns>
    public RepostCheep CreateReCheep(Author author, OriginalCheep originalCheep, DateTime timestamp);

    /// <summary>
    /// Retrieve the original <c>Cheep</c> associated with the specified ID.
    /// </summary>
    /// <param name="cheepId">
    /// The unique identifier of the <c>Cheep</c> to retrieve.
    /// </param>
    /// <returns>
    /// The <c>OriginalCheep</c> with the specified ID if found.
    /// </returns>
    public OriginalCheep GetOriginalCheepById(int cheepId);
}