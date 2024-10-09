using Chirp.Core;
using Chirp.Core.DataTransferObjects;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepDTO> GetCheeps(int page = 1);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1);
}

/// <summary>
/// Provides methods for retrieving Cheeps.
/// </summary>
public class CheepService : ICheepService
{
    private readonly ICheepRepository _database;
    private readonly int _pageSize = 32;

    public CheepService(ICheepRepository database)
    {
        _database = database;
    }

    /// <summary>
    /// Method <c>GetCheeps</c> returns a list of CheepDTO's.
    /// </summary>
    /// <param name="page"> The page we want to display, if nothing it defaults to 1</param>
    /// <returns> A List of CheepDTO's</returns>
    public List<CheepDTO> GetCheeps(int page = 1)
    {
        var skip = _pageSize * (page - 1);
        var size = _pageSize;
        return _database
            .GetCheeps(skip, size)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    /// <summary>
    ///  Method <c>GetCheepsFromAuthor</c> returns a list of CheepDTO's from a specific author.
    /// </summary>
    /// <param name="author">The name of the Author, whose cheeps we want</param>
    /// <param name="page"> The page we want to display, if nothing it defaults to 1 </param>
    /// <returns></returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        var skip = _pageSize * (page - 1);
        return _database
            .GetCheeps(skip, _pageSize, author)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    /// <summary>
    /// Method <c>GetCheepViewModels</c> makes a unixTimeStamp into a string.
    /// </summary>
    /// <param name="unixTimeStamp">The unixTimeStamp we want as a string</param>
    /// <returns></returns>
    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}