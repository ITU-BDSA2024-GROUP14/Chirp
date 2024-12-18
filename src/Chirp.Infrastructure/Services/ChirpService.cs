using System.Data;
using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Data.DataTransferObjects;
using Chirp.Infrastructure.Repositories;

namespace Chirp.Infrastructure.Services;
/// <summary>
/// This class is used as a service to use in the presentation layer of the application to interact with the database.
/// </summary>
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
    /// <summary>
    /// This method is used to get a list of CheepDTO objects that correspond to the cheeps in the database
    /// </summary>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheeps(int page = 1)
    {
        var skip = PageSize * (page - 1);
        return _cheepRepository
            .GetCheeps(skip, PageSize)
            .Select(x => new CheepDTO(x))
            .ToList();
    }
    /// <summary>
    /// This method is used to get a list of CheepDTO objects from a specific author
    /// that correspond to the cheeps in the database.
    /// </summary>
    /// <param name="author">The author of the cheeps to get from the database</param>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page = 1)
    {
        List<string> tempList = [author];
        return GetCheepsFromMultipleAuthors(tempList, page);
    }
    /// <summary>
    /// This method is used to get a list of CheepDTO objects from multiple authors from the database.
    /// </summary>
    /// <param name="authorList">The list of the authors you want to get cheeps from</param>
    /// <param name="page">The page number used to determine which cheeps are picked from the database</param>
    /// <returns>A list of CheepDTO</returns>
    public List<CheepDTO> GetCheepsFromMultipleAuthors(List<string> authorList, int page = 1)
    {
        // filter by the provided author name
        var skip = PageSize * (page - 1);
        return _cheepRepository
            .GetCheepsByAuthor(skip: skip, size: PageSize, authorUsernameList: authorList)
            .Select(x => new CheepDTO(x))
            .ToList();
    }
    /// <summary>
    /// This method is used to create a cheep in the database.
    /// </summary>
    /// <param name="authorName">The authors name of the cheep</param>
    /// <param name="authorEmail">The authors email of the cheep</param>
    /// <param name="text">The text that should appear on the cheep</param>
    /// <param name="timestamp">The timestamp the cheep was cheeped at</param>
    public void CreateCheep(string authorName, string authorEmail, string text, DateTime timestamp)
    {
        var author = _authorRepository.GetAuthorByName(authorName) ??
                     _authorRepository.CreateAuthor(authorName, authorEmail);

        _cheepRepository.CreateCheep(author, text, timestamp);
    }
    /// <summary>
    /// This method is used to get an AuthorDTO object from the database by the authors name.
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <returns>A data transfer object of the requested author</returns>
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
    /// <summary>
    /// This method is used to get an AuthorDTO object from the database by the authors email.
    /// </summary>
    /// <param name="authorEmail">The authors email</param>
    /// <returns>A data transfer object of the requested author</returns>
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
    /// <summary>
    /// This method is used to create an author in the database.
    /// </summary>
    /// <param name="authorName">The authors name</param>
    /// <param name="authorEmail">The authors email</param>
    public void CreateAuthor(string authorName, string authorEmail)
    {
        _authorRepository.CreateAuthor(authorName, authorEmail);
    }
    /// <summary>
    /// This method is used to follow a user in the database.
    /// </summary>
    /// <param name="authorName">The author that wants to follow another author</param>
    /// <param name="toFollowAuthorName">The author that is to be followed</param>
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
    /// <summary>
    /// This method is used to unfollow a user in the database.
    /// </summary>
    /// <param name="authorName">The name of the author that wants to unfollow another author</param>
    /// <param name="toUnFollowAuthorName">The name of the author that is to be unfollowed</param>
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
    /// <summary>
    /// This method is used to get a list of authors names that a specific author follows.
    /// </summary>
    /// <param name="loggedInDisplayName">The name of the author from which to get the author it follows</param>
    /// <returns>A list of string corresponding to all the authors that are being followed</returns>
    public List<string> GetFollowing(string loggedInDisplayName)
    {
        return _authorRepository.GetFollowing(loggedInDisplayName);
    }
    /// <summary>
    /// This method is used to create a ReCheep in the database.
    /// </summary>
    /// <param name="authorName">The author that is recheeping a cheep</param>
    /// <param name="cheepId">The id of the cheep that is to be recheeped</param>
    public void ReCheep(string authorName, int cheepId)
    {
        var author = _authorRepository.GetAuthorByName(authorName);
        if (author is null)
        {
            throw new AuthorMissingException(authorName);
        }

        var originalPost = _cheepRepository.GetOriginalCheepById(cheepId);

        _cheepRepository.CreateReCheep(author, originalPost, DateTime.Now);
    }

    /// <summary>
    /// This method is used to check if an author is following another author.
    /// </summary>
    /// <param name="authorName">The name of the author that might be following someone</param>
    /// <param name="followingAuthorName">The name of the author that might be followed</param>
    /// <returns>true or false if they are following or not</returns>
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