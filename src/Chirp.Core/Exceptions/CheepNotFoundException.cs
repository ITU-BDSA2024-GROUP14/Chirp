namespace Chirp.Core.Exceptions;
/// <summary>
/// This exception is thrown when the application is looking for a specific Cheep in the database and cant find it.
/// </summary>
public class CheepNotFoundException : Exception
{
    public CheepNotFoundException(int cheepId) : base($"A cheep with ID {cheepId} does not exist")
    {
    }
}