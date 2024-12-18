namespace Chirp.Core.Exceptions;

public class CheepNotFoundException : Exception
{
    public CheepNotFoundException(int cheepId) : base($"A cheep with ID {cheepId} does not exist")
    {
    }
}