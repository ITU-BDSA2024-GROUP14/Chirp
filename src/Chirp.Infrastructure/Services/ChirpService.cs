using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Repositories;

namespace Chirp.Infrastructure.Services;

public class ChirpService : IChirpService
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public const int PageSize = 32;

    public ChirpService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public List<CheepDTO> GetCheeps(int page = 1)
    {
        var skip = PageSize * (page - 1);
        return _cheepRepository
            .GetCheeps(skip, PageSize)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1)
    {
        // filter by the provided author name
        var skip = PageSize * (page - 1);
        return _cheepRepository
            .GetCheepsByAuthor(skip: skip, size: PageSize, author: author)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp)
    {
        var author = _authorRepository.GetAuthorByName(authorName) ??
                     _authorRepository.CreateAuthor(authorName, authorEmail);

        _cheepRepository.CreateCheep(author, text, timestamp);
    }

    /*private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp //is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }*/

    public AuthorDTO? GetAuthorByName(string authorName)
    {
        var author = _authorRepository.GetAuthorByName(authorName);
        if (author == null)
        {
            return null;
        }

        var dto = new AuthorDTO { Name = author.Name, Email = author.Email };
        return dto;
    }

    public AuthorDTO? GetAuthorByEmail(string authorEmail)
    {
        var author = _authorRepository.GetAuthorByEmail(authorEmail);
        if (author == null)
        {
            return null;
        }

        var dto = new AuthorDTO { Name = author.Name, Email = author.Email };
        return dto;
    }

    public void CreateAuthor(string authorName, string authorEmail)
    {
        _authorRepository.CreateAuthor(authorName, authorEmail);
    }
}