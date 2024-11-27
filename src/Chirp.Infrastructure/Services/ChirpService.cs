using System.Data;
using Chirp.Core.Exceptions;
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
        List<string> tempList = [author];
        return GetCheepsFromMultipleAuthors(tempList, page);
    }

    public List<CheepDTO> GetCheepsFromMultipleAuthors(List<string> authorList, int page = 1)
    {
        // filter by the provided author name
        var skip = PageSize * (page - 1);
        return _cheepRepository
            .GetCheepsByAuthor(skip: skip, size: PageSize, authorUsernameList: authorList)
            .Select(x => new CheepDTO(x))
            .ToList();
    }

    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp)
    {
        var author = _authorRepository.GetAuthorByName(authorName) ??
                     _authorRepository.CreateAuthor(authorName, authorEmail);

        _cheepRepository.CreateCheep(author, text, timestamp);
    }

    public AuthorDTO? GetAuthorByName(string authorName)
    {
        var author = _authorRepository.GetAuthorByName(authorName);
        if (author == null)
        {
            return null;
        }

        var dto = new AuthorDTO(author);
        return dto;
    }

    public AuthorDTO? GetAuthorByEmail(string authorEmail)
    {
        var author = _authorRepository.GetAuthorByEmail(authorEmail);
        if (author == null)
        {
            return null;
        }

        var dto = new AuthorDTO(author);
        return dto;
    }

    public void CreateAuthor(string authorName, string authorEmail)
    {
        _authorRepository.CreateAuthor(authorName, authorEmail);
    }

    public void FollowUser(string userName, string toFollowAuthorName)
    {
        if (userName.Equals((toFollowAuthorName)))
        {
            throw new ArgumentException("User cannot follow themself.");
        }
        var user = _authorRepository.GetAuthorByName(userName);
        if (user == null)
        {
            throw new NoNullAllowedException("Logged in user does not exist.");
        }

        var toFollow = _authorRepository.GetAuthorByName(toFollowAuthorName);
        if (toFollow == null)
        {
            throw new NoNullAllowedException("User to be followed does not exist.");
        }

        _authorRepository.FollowUser(user, toFollow);
    }

    public void UnFollowUser(string authorName, string toUnFollowAuthorName)
    {
        var user = _authorRepository.GetAuthorByName(authorName);
        if (user == null)
        {
            throw new NoNullAllowedException("Logged in user does not exist.");
        }

        var toUnFollow = _authorRepository.GetAuthorByName(toUnFollowAuthorName);
        if (toUnFollow == null)
        {
            throw new NoNullAllowedException("User to be followed does not exist.");
        }

        _authorRepository.UnFollowUser(user, toUnFollow);
    }

    public List<string> GetFollowing(string loggedInDisplayName)
    {
        return _authorRepository.GetFollowing(loggedInDisplayName);
    }

    public void RepostCheep(string authorName, int cheepId)
    {
        var author = _authorRepository.GetAuthorByName(authorName);
        if (author is null)
        {
            throw new AuthorMissingException(authorName);
        }

        var originalPost = _cheepRepository.GetOriginalCheepById(cheepId);
        if (originalPost is null)
        {
            throw new CheepNotFoundException(cheepId);
        }
        
        _cheepRepository.CreateRepost(author, originalPost, DateTime.Now);
    }


    public bool CheckIfFollowing(string authorName, string followingAuthorName)
    {
        var following = _authorRepository.GetFollowing(authorName);
        var author = _authorRepository.GetAuthorByName(followingAuthorName);
        if (author is null)
        {
            throw new NullReferenceException("User does not exist.");
        }

        return following.Contains(author.DisplayName);
    }
}