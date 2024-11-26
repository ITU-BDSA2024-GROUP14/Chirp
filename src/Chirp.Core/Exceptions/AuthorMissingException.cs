namespace Chirp.Core.Exceptions;

public class AuthorMissingException : Exception
{
    public AuthorMissingException(string authorName) : base($"The author {authorName} does not exist")
    {
    }
}