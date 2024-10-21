using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Core.DataTransferObjects;
using Chirp.Razor.Exceptions;

namespace Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface IChirpService
{
    public List<CheepDTO> GetCheeps(int page = 1);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1);
    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp);
    public AuthorDTO? GetAuthor(string? authorName = null, string? authorEmail = null);
    public void CreateAuthor(string authorName, string authorEmail);
}

public class ChirpService : IChirpService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly int _pageSize = 32;
    private readonly IAuthorRepository _authorRepository;

    public ChirpService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public List<CheepDTO> GetCheeps(int page = 1)
    {
        int skip = _pageSize * (page - 1);
        int size = _pageSize;
        return _cheepRepository
            .GetCheeps(skip: skip, size: size)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        int skip = _pageSize * (page - 1);
        return _cheepRepository
            .GetCheeps(skip: skip, size: _pageSize, authorUsername: author)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp)
    {
        var author = _authorRepository.GetAuthor(authorName, authorEmail);
        if (author == null)
        {
            author = _authorRepository.CreateAuthor(authorName, authorEmail);
        }
        _cheepRepository.CreateCheep(author, text, timestamp);

    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
    public AuthorDTO? GetAuthor(string? authorName = null, string? authorEmail = null)
    {
        var author = _authorRepository.GetAuthor(authorName: authorName, authorEmail: authorEmail);

        if (author == null)
        {
            return null;
        }

        var dto = new AuthorDTO {Name = author.Name, Email = author.Email};
        return dto;
    }
    
    public void CreateAuthor(string authorName, string authorEmail)
    {
        if (GetAuthor(authorName, authorEmail) == null)
        {
            throw new DuplicateEntryException("Author with email or name already exists\nName: " + authorName +
                                              "\nEmail: " + authorEmail);
        }
        _authorRepository.CreateAuthor(authorName: authorName, authorEmail: authorEmail);
    }
}