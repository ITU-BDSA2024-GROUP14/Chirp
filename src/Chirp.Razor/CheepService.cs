using Chirp.Core;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page = 1);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page = 1);
}

public class CheepService : ICheepService
{
    private readonly DBFacade _dbFacade;

    public CheepService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public List<CheepViewModel> GetCheeps(int page = 1)
    {
        return _dbFacade
            .GetCheeps(page)
            .Select(x => new CheepViewModel(x.Author, x.Text, UnixTimeStampToDateTimeString(x.Timestamp)))
            .ToList();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        return _dbFacade.GetCheeps(page, author)
            .Select(x => new CheepViewModel(x.Author, x.Text, UnixTimeStampToDateTimeString(x.Timestamp)))
            .ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}