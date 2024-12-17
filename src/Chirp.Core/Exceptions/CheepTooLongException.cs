using Chirp.Core.DataModel;

namespace Chirp.Core.Exceptions;
/// <summary>
/// This exception is thrown whenever a cheep that contains too many characters is tried to saved in the database.
/// </summary>
public class CheepTooLongException : Exception
{
    private Cheep cheep;
    public CheepTooLongException(Cheep cheep)
    {
        this.cheep = cheep;
    }
}