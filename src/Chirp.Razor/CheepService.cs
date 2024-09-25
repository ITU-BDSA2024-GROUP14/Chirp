using Chirp.Core;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly DBFacade _dbFacade;

    public CheepService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public List<CheepViewModel> GetCheeps()
    {
        return _dbFacade
            .GetCheeps()
            .Select(x => new CheepViewModel(x.Author, x.Message, UnixTimeStampToDateTimeString(x.Timestamp)))
            .ToList();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _dbFacade.GetCheeps(author)
            .Select(x => new CheepViewModel(x.Author, x.Message, UnixTimeStampToDateTimeString(x.Timestamp)))
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