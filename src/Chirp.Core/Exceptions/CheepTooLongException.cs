using Chirp.Core.DataModel;

namespace Chirp.Core.Exceptions;

public class CheepTooLongException : Exception
{
    private Cheep cheep;
    public CheepTooLongException(Cheep cheep)
    {
        this.cheep = cheep;
    }
}