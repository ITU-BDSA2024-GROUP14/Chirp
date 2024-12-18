namespace Chirp.Core.Exceptions;

/// <summary>
///     This exception is thrown when the application is looking for a specific author in the database and it cannot find
///     it.
/// </summary>
public class AuthorMissingException : Exception
{
    public AuthorMissingException(string authorName) : base($"The author {authorName} does not exist")
    {
    }
}