using Chirp.Core.DataModel;

namespace Chirp.Core.Exceptions;

/// <summary>
///     This exception is thrown whenever there is an attempt to save a cheep that contains too many characters in the
///     database.
/// </summary>
public class CheepTooLongException : Exception
{
    private Cheep cheep;

    public CheepTooLongException(Cheep cheep)
    {
        this.cheep = cheep;
    }
}