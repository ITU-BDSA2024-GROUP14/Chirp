using Chirp.Core;
using Chirp.Core.DataTransferObjects;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepDTO> GetCheeps(int page = 1);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _database;
    private readonly int _pageSize = 32;

    public CheepService(ICheepRepository database)
    {
        _database = database;
    }

    public List<CheepDTO> GetCheeps(int page = 1)
    {
        int skip = _pageSize * (page - 1);
        int size = _pageSize;
        return _database
            .GetCheeps(skip: skip, size: size)
            //.Select(x => new CheepViewModel(x.Author, x.Text, UnixTimeStampToDateTimeString(x.Timestamp)))
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        //return _database.GetCheeps(page, author)
        //    .Select(x => new CheepViewModel(x.Author, x.Text, UnixTimeStampToDateTimeString(x.Timestamp)))
        //    .ToList();
        throw new NotImplementedException();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}