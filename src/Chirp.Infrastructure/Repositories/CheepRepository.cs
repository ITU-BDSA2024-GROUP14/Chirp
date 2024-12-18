using System.Data;
using Chirp.Core;
using Chirp.Core.DataModel;
using Chirp.Core.Exceptions;
using Chirp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Repositories;

/// <summary>
/// Provides methods for interacting with the Cheep entities in the database.
/// Implements the <c>ICheepRepository</c> interface.
/// </summary>
public class CheepRepository : ICheepRepository
{
    private ChirpDBContext _dbcontext;

    public CheepRepository(ChirpDBContext context)
    {
        _dbcontext = context;
    }

    /// <summary>
    /// Get cheeps from the database.
    /// </summary>  
    /// <param name="skip">The number of Cheeps to skip.</param>
    /// <param name="size">The number of Cheeps to return.</param>
    /// <returns>Specified cheeps from the database.</returns>
    public IEnumerable<Cheep> GetCheeps(int skip = 0, int? size = null)
    {
        var query = _dbcontext.Cheeps
            .Include(cheep => cheep.Author)
            .Include(cheep => (cheep as RepostCheep)!.Content)
            .Include(cheep => (cheep as RepostCheep)!.Content.Author)
            .AsQueryable();
        query = query.OrderByDescending(cheep => cheep.TimeStamp);

        query = query.Skip(skip);

        if (size != null)
        {
            query = query.Take((int)size);
        }

        return query.ToList();
    }

    /// <summary>
    /// Get cheeps from the database by specified author.
    /// </summary>
    /// <param name="authorUsernameList">The list of author usernames whose cheeps should be retrieved.</param>
    /// <param name="skip">The number of cheeps to skip in the results. Default is 0.</param>
    /// <param name="size">The maximum number of cheeps to return. If null, all remaining cheeps are returned.</param>
    /// <returns>A collection of cheeps authored by the specified authors, ordered by descending timestamp, with optional pagination applied.</returns>
    public IEnumerable<Cheep> GetCheepsByAuthor(List<string> authorUsernameList, int skip = 0, int? size = null)
    {
        var query = _dbcontext.Cheeps
            .Include(cheep => cheep.Author)
            .AsQueryable()
            .Include(cheep => (cheep as RepostCheep)!.Content)
            .Include(cheep => (cheep as RepostCheep)!.Content.Author)
            .AsQueryable();
        query = query.Where(cheep => authorUsernameList.Contains(cheep.Author.DisplayName));
        query = query.OrderByDescending(cheep => cheep.TimeStamp);
        query = query.Skip(skip);

        if (size != null)
        {
            query = query.Take((int)size);
        }

        return query.ToList();
    }

    /// <summary>
    /// Create a new Cheep and add it to the repository.
    /// </summary>
    /// <param name="author">The author of the Cheep.</param>
    /// <param name="text">The text content of the Cheep.</param>
    /// <param name="timestamp">The creation timestamp of the Cheep.</param>
    /// <returns>The created Cheep object.</returns>
    /// <exception cref="CheepTooLongException">Thrown when the text length exceeds the maximum allowed.</exception>
    public OriginalCheep CreateCheep(Author author, string text, DateTime timestamp)
    {
        var cheep = new OriginalCheep() { Author = author, Text = text, TimeStamp = timestamp };
        if (cheep.GetText().Length > Cheep.MaxLength)
        {
            throw new CheepTooLongException(cheep);
        }

        _dbcontext.Cheeps.Add(cheep);
        _dbcontext.SaveChanges();
        return cheep;
    }

    /// <summary>
    /// Create a new re-cheep (repost) from an existing cheep.
    /// </summary>
    /// <param name="author">The author creating the re-cheep.</param>
    /// <param name="originalCheep">The original cheep being re-cheeped.</param>
    /// <param name="timestamp">The timestamp of when the re-cheep is created.</param>
    /// <returns>A newly created re-cheep associated with the original cheep.</returns>
    public RepostCheep CreateReCheep(Author author, OriginalCheep originalCheep, DateTime timestamp)
    {
        var cheep = new RepostCheep
        {
            Author = author,
            TimeStamp = timestamp,
            Content = originalCheep
        };
        _dbcontext.Cheeps.Add(cheep);
        _dbcontext.SaveChanges();
        return cheep;
    }

    /// <summary>
    /// Retrieves the original cheep associated with the specified ID.
    /// </summary>
    /// <param name="cheepId">The ID of the cheep to retrieve.</param>
    /// <returns>The original cheep with the specified ID.</returns>
    /// <exception cref="CheepNotFoundException">Thrown when no cheep with the specified ID is found or the cheep is not an original cheep.</exception>
    public OriginalCheep GetOriginalCheepById(int cheepId)
    {
        var cheep = _dbcontext.Cheeps
            .Include(c => (c as RepostCheep)!.Content)
            .FirstOrDefault(c => c.CheepId == cheepId);

        while (cheep is RepostCheep repostCheep)
        {
            cheep = repostCheep.Content;
        }
        
        if (cheep == null || cheep is not OriginalCheep originalCheep)
        {
            throw new CheepNotFoundException(cheepId);
        }

        return originalCheep;
    }
}